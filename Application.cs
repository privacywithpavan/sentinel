namespace Sentinel;

public sealed class Application
{
    private readonly IModelService _modelService;

    public Application(IModelService modelService)
    {
        _modelService = modelService;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var rootCommand = new RootCommand(description: "A simple command-line application to interact with Large Language Models (LLMs).");
        rootCommand.SetAction(_ => HandleRootCommand());

        var invokeCommand = new InvokeCommand(_modelService, name: "invoke", description: "Invoke a command to chat with the assistant.");
        rootCommand.Subcommands.Add(invokeCommand);

        var parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count == 0)
        {
            await parseResult.InvokeAsync();
            return 0;
        }

        var errorMessages = parseResult.Errors.Select(e => e.Message).ToArray();
        ConsoleHelper.ShowErrorMessages(errorMessages);
        return 1;
    }

    private static void HandleRootCommand()
    {
        AnsiConsole.Write(new FigletText("Sentinel").Centered().
        Color(Color.SteelBlue));
    }
}
