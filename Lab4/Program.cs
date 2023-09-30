using System.Text;
using Spectre.Console;

//
// Summary:
//   strip every bit at the end of the byte to get a 7 byte sequence
//   check via https://www.rapidtables.com/convert/number/ascii-to-binary.html
//         and https://www.rapidtables.com/convert/number/hex-to-binary.html
byte[] GetDESKey(string inputKey)
{
    if (inputKey.Length != 8)
        throw new ArgumentException("The key must be 8 characters long");

    byte[] key = Encoding.ASCII
        .GetBytes(inputKey)
        .Select(b => (byte)(b & 0b11111110))
        .ToArray();
    byte[] desKey = new byte[7];
    for (int i = 0; i < 7; i++)
    {
        byte b1 = key[i];
        byte b2 = key[i + 1];
        byte b1Part = (byte)(b1 << i);
        byte b2Part = (byte)(b2 >> (7 - i));
        desKey[i] = (byte)(b1Part | b2Part);
    }
    return desKey;
}

string FormatToHex(byte[] bytes) => string.Join(
    "-",
    Convert
        .ToHexString(bytes)
        .ToCharArray()
        .Select((character, index) => (character, index))
        .GroupBy(tuple => tuple.index / 2)
        .Select(group => new string(group.Select(tuple => tuple.character).ToArray()))
);

Console.Clear();
AnsiConsole.MarkupLine(
    "[cyan]Lab 4[/] [green]Given K+ in the algorithm DES, find C(i) and D(i) for a given i[/]"
);

Console.WriteLine(Encoding.ASCII.GetBytes("12345678").Length);

string key = AnsiConsole.Prompt(
    new TextPrompt<string>("Enter the key:")
        .PromptStyle("red")
        .Validate(text =>
        {
            if (text.Length != 8)
                return ValidationResult.Error("[red]The key must be 8 characters long[/]");
            if (text.Any(c => !char.IsAscii(c)))
                return ValidationResult.Error("[red]The key must contain only ascii characters[/]");
            return ValidationResult.Success();
        })
);
string base16Key = FormatToHex(Encoding.ASCII.GetBytes(key));
byte[] desKey = GetDESKey(key);
string base16DESKey = FormatToHex(desKey);
AnsiConsole.MarkupLine($"[green]Hex K:[/] {base16Key}");
AnsiConsole.MarkupLine($"[green]Hex K+:[/] {base16DESKey}");
