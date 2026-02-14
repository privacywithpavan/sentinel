namespace Sentinel.Options;

public class OpenAIOption
{
    public required string BaseUrl { get; set; }

    public required int Version { get; set; }

    public required EndpointsOption Endpoints { get; set; }

    public required string Model { get; set; }
}

public class EndpointsOption
{
    public required string TextGeneration { get; set; }
}
