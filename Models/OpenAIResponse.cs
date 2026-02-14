namespace Sentinel.Models;

public sealed class OpenAIResponse
{
    public required IEnumerable<ResponseOutputItem> Output { get; init; }
}

public sealed class ResponseOutputItem
{
    public required IEnumerable<ResponseOutputText> Content { get; init; }
}

public sealed class ResponseOutputText
{
    public required string Text { get; init; }
}
