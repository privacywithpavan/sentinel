namespace Sentinel.Models;

public sealed class OpenAIRequest
{
    public required string Model { get; init; }
    public required string Input { get; init; }
}
