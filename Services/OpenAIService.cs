namespace Sentinel.Services;

public sealed class OpenAIService : IModelService
{
    private readonly OpenAIOption _options;
    private readonly IHttpClientFactory _clientFactory;

    public OpenAIService(IOptions<OpenAIOption> options, IHttpClientFactory clientFactory)
    {
        _options = options.Value;
        _clientFactory = clientFactory;
    }

    public async Task<string?> InvokeAsync(string prompt, CancellationToken cancellationToken)
    {
        try
        {
            var requestUrl = $"/v{_options.Version}/{_options.Endpoints.TextGeneration}";

            var client = _clientFactory.CreateClient("OpenAI");
            
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(
                        new OpenAIRequest
                        {
                            Model = _options.Model,
                            Input = prompt
                        },
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    ),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await client.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<OpenAIResponse>(
                content,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            return result?.Output.FirstOrDefault()?.Content?.FirstOrDefault()?.Text;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error invoking OpenAI API: {ex.Message}[/]");
            return null;
        }
    }
}
