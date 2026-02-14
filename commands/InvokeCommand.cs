namespace Sentinel.Commands;

public class InvokeCommand : Command
{
    public InvokeCommand(string name, string? description = null) : base(name, description)
    {
        var promptOption = new Option<string>(name: "--prompt", aliases: ["-p"])
        {
            Description = "The prompt to send to the assistant",
            Required = true
        };

        promptOption.Validators.Add(result =>
        {
            if(string.IsNullOrWhiteSpace(result.GetValue(promptOption)))
            {
                result.AddError("Prompt cannot be empty.");
            }
        });

        Options.Add(promptOption);

        SetAction(async (parseResult, cancellationToken) =>
        {
            var prompt = parseResult.GetValue(promptOption);
            await HandleInvokeCommand(prompt, cancellationToken);
        });
    }

    static async Task HandleInvokeCommand(string? prompt, CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.BouncingBall)
            .StartAsync("Processing your request...", async ctx =>
            {
                await Task.Delay(3000, cancellationToken);
            });

        var response = "Hello, how can I assist you today?";

        var panel = new Panel(
            new Rows(
                new Markup($"{Emoji.Known.Person}: {prompt}"),
                new Markup($"{Emoji.Known.Robot}: {response}")
            )
        ).Header("[yellow]Chat Summary[/]").RoundedBorder();

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }
}
