using System;
using CS.Lab1;
using Spectre.Console;

const string CLASSIC_CAESAR = "Classic Caesar";
const string CAESAR_PLUS_PERMUTATION = "Caesar + Permutation";
const string TEST_CUSTOM_DICTIONARY = "Test custom dictionary";
const string ENCRYPT = "Encrypt";
const string DECRYPT = "Decrypt";

string PromptEncryptOrDecrypt() => AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("[green]Select an action[/]")
        .AddChoices(new[] { ENCRYPT, DECRYPT })
);

string PromptDictionaryKey() => AnsiConsole.Prompt(
    new TextPrompt<string>("Enter the dictionary key:")
        .PromptStyle("red")
        .Validate(text =>
        {
            string normalizedText = CaesarAlgorithm.NormalizeText(text);
            if (normalizedText.Length == 0)
                return ValidationResult.Error("[red]The dictionary key cannot be empty[/]");
            if (normalizedText.Any(c => !char.IsLetter(c)))
                return ValidationResult.Error("[red]The dictionary key must contain only letters[/]");
            if (normalizedText.Length < 7)
                return ValidationResult.Error("[red]The dictionary key be >= 7 characters long[/]");
            return ValidationResult.Success();
        })
);

ValidationResult ValidateInputText(string text)
{
    string normalizedText = CaesarAlgorithm.NormalizeText(text);
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
        .Secret()
        .Validate(ValidateInputText)
);

int PromptKey() => AnsiConsole.Prompt(
    new TextPrompt<int>("Enter the key:")
        .PromptStyle("red")
        .ValidationErrorMessage("[red]The key must be a number[/]")
        .Validate(key =>
            key switch
            {
                < 0 => ValidationResult.Error("[red]The key must be positive[/]"),
                >= 26 => ValidationResult.Error("[red]The key must be less than 26[/]"),
                _ => ValidationResult.Success()
            }
        )
);

void ExecuteClassicCaesar()
{
    AnsiConsole.MarkupLine("[green]Classic Caesar[/]");
    string action = PromptEncryptOrDecrypt();
    string text = action switch
    {
        ENCRYPT => PromptPlainText(),
        DECRYPT => PromptEncryptedText(),
        _ => throw new Exception("Invalid action")
    };

    string normalizedText = CaesarAlgorithm.NormalizeText(text);
    int key = PromptKey();

    CaesarAlgorithm caesar = new();
    switch (action)
    {
        case ENCRYPT:
            string cipherText = caesar.Encrypt(normalizedText, key);
            AnsiConsole.MarkupLine($"[green]Cipher text:[/] {cipherText}");
            break;
        case DECRYPT:
            string decryptedText = caesar.Decrypt(normalizedText, key);
            AnsiConsole.MarkupLine($"[green]Decrypted text:[/] {decryptedText}");
            break;
    }
}

void ExecuteCaesarPlusPermutation()
{
    AnsiConsole.MarkupLine("[green]Caesar + Permutation[/]");
    string action = PromptEncryptOrDecrypt();
    string text = action switch
    {
        ENCRYPT => PromptPlainText(),
        DECRYPT => PromptEncryptedText(),
        _ => throw new Exception("Invalid action")
    };

    string normalizedText = CaesarAlgorithm.NormalizeText(text);
    string dictionaryKey = PromptDictionaryKey();
    string customDictionary = CaesarAlgorithm.GetCustomDictionary(dictionaryKey);
    int key = PromptKey();

    CaesarAlgorithm caesar = new(customDictionary);
    switch (action)
    {
        case ENCRYPT:
            string cipherText = caesar.Encrypt(normalizedText, key);
            AnsiConsole.MarkupLine($"[green]Cipher text:[/] {cipherText}");
            break;
        case DECRYPT:
            string decryptedText = caesar.Decrypt(normalizedText, key);
            AnsiConsole.MarkupLine($"[green]Decrypted text:[/] {decryptedText}");
            break;
    }
}

void ExecuteTestCustomDictionary()
{
    string dictionaryKey = PromptDictionaryKey();
    string customDictionary = CaesarAlgorithm.GetCustomDictionary(dictionaryKey);
    AnsiConsole.MarkupLine($"[green]Custom dictionary:[/] {customDictionary}");
}

Dictionary<string, Action> algorithms = new()
{
    { CLASSIC_CAESAR, ExecuteClassicCaesar },
    { CAESAR_PLUS_PERMUTATION, ExecuteCaesarPlusPermutation },
    { TEST_CUSTOM_DICTIONARY, ExecuteTestCustomDictionary}
};

Console.Clear();
string selectedAlgorithm = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("[green]Select an algorithm[/]")
        .AddChoices(new[] { CLASSIC_CAESAR, CAESAR_PLUS_PERMUTATION, TEST_CUSTOM_DICTIONARY })
);

Console.Clear();
algorithms[selectedAlgorithm]();