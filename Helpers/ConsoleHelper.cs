namespace Sentinel.Helpers;

public static class ConsoleHelper
{
    public static void ShowErrorMessages(string[] errorMessages)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        AnsiConsole.MarkupLineInterpolated($"{Emoji.Known.CrossMark} [bold red]ERROR[/] {Emoji.Known.CrossMark}");

        foreach (var message in errorMessages)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]- {message}[/]");
        }
    }
}
