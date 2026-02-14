namespace Sentinel.Commands;

public sealed class InvokeCommand : Command
{
    private readonly IModelService _modelService;

    public InvokeCommand(IModelService modelService, string name, string? description = null) : base(name, description)
    {
        _modelService = modelService;

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
            await HandleInvokeCommand(prompt!, cancellationToken);
        });
    }

    private async Task HandleInvokeCommand(string prompt, CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        var response = await AnsiConsole.Status()
            .Spinner(Spinner.Known.BouncingBall)
            .StartAsync("Processing your request...", async ctx =>
            {
                return await _modelService.InvokeAsync(prompt, cancellationToken);
            });

        var panel = new Panel(
            new Rows(
                new Markup($"{Emoji.Known.Person}: {prompt}\n"),
                new Markup($"{Emoji.Known.Robot}: {response}\n")
            )
        ).Header("[yellow]Chat Summary[/]").RoundedBorder();

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }
}
