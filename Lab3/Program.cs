using CS.Lab3;
using Spectre.Console;
const string ENCRYPT = "Encrypt";
const string DECRYPT = "Decrypt";

string PromptEncryptOrDecrypt() => AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("[green]Select an action[/]")
        .AddChoices(new[] { ENCRYPT, DECRYPT })
);

string PromptKey() => AnsiConsole.Prompt(
    new TextPrompt<string>("Enter the encryption key:")
        .PromptStyle("red")
        .Validate(text =>
        {
            string normalizedText = VigenereAlgorithm.NormalizeText(text);
            if (normalizedText.Length == 0)
                return ValidationResult.Error("[red]The dictionary key cannot be empty[/]");
            if (normalizedText.Any(c => !char.IsLetter(c)))
                return ValidationResult.Error("[red]The encryption key must contain only letters[/]");
            if (normalizedText.Length < 7)
                return ValidationResult.Error("[red]The encryption key must be >= 7 characters long[/]");
            return ValidationResult.Success();
        })
);

ValidationResult ValidateInputText(string text)
{
    string normalizedText = VigenereAlgorithm.NormalizeText(text);
    if (normalizedText.Length == 0)
        return ValidationResult.Error("[red]The text cannot be empty[/]");
    if (normalizedText.Any(c => !char.IsLetter(c)))
        return ValidationResult.Error("[red]The text must contain only letters[/]");
    return ValidationResult.Success();
}

string PromptPlainText() => AnsiConsole.Prompt(
    new TextPrompt<string>("Enter the plain text:")
        .PromptStyle("red")
        .Secret()
        .Validate(ValidateInputText)
);

string PromptEncryptedText() => AnsiConsole.Prompt(
    new TextPrompt<string>("Enter the encrypted text:")
        .PromptStyle("red")
        .Validate(ValidateInputText)
);

void ExecuteVigenereAlgorithm()
{
    AnsiConsole.MarkupLine("[green]Vigenere Algorithm[/]");
    string action = PromptEncryptOrDecrypt();
    string text = action switch
    {
        ENCRYPT => PromptPlainText(),
        DECRYPT => PromptEncryptedText(),
        _ => throw new Exception("Invalid action")
    };

    string normalizedText = VigenereAlgorithm.NormalizeText(text);
    string key = PromptKey();

    VigenereAlgorithm vigenere = new(key);
    switch (action)
    {
        case ENCRYPT:
            string cipherText = vigenere.Encrypt(text);
            AnsiConsole.MarkupLine($"[green]Cipher text:[/] {cipherText}");
            break;
        case DECRYPT:
            string decryptedText = vigenere.Decrypt(text);
            AnsiConsole.MarkupLine($"[green]Decrypted text:[/] {decryptedText}");
            break;
    }
}


Console.Clear();
ExecuteVigenereAlgorithm();