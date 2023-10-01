using System.Text;
using CS.Lab4;
using Spectre.Console;
using static CS.Common.Formatting;

const int HALF_KEY_NIBBLES = 7;

Console.Clear();
AnsiConsole.MarkupLine(
    "[cyan]Lab 4[/] [green]Given K+ in the algorithm DES, find C(i) and D(i) for a given i[/]"
);

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
byte[] desKey = DESHelpers.GetDESKey(key);
string base16DESKey = FormatToHex(desKey);
AnsiConsole.MarkupLine($"[green]Hex K:[/] {base16Key}");
AnsiConsole.MarkupLine($"[green]Hex K+:[/] {base16DESKey}");
AnsiConsole.WriteLine();

var (cKey, dKey) = DESHelpers.GetCAndDKeys(desKey, 0);
AnsiConsole.MarkupLine($"[green]Hex C(0):[/] {FormatToHex(cKey, HALF_KEY_NIBBLES)}");
AnsiConsole.MarkupLine($"[green]Hex D(0):[/] {FormatToHex(dKey, HALF_KEY_NIBBLES)}");
AnsiConsole.WriteLine();

int round = AnsiConsole.Prompt(
    new TextPrompt<int>("Enter the round:")
        .PromptStyle("red")
        .Validate(text =>
        {
            if (text < 1 || text > 16)
                return ValidationResult.Error("[red]The round must be between 1 and 16[/]");
            return ValidationResult.Success();
        })
);
var (cKeyI, dKeyI) = DESHelpers.GetCAndDKeys(desKey, round);
AnsiConsole.MarkupLine($"[green]Hex C({round}):[/] {FormatToHex(cKeyI, HALF_KEY_NIBBLES)}");
AnsiConsole.MarkupLine($"[green]Hex D({round}):[/] {FormatToHex(dKeyI, HALF_KEY_NIBBLES)}");