namespace Sentinel;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Logging.ClearProviders();

        builder.Services.Configure<OpenAIOption>(
            builder.Configuration.GetSection("OpenAI")
        );

        builder.Services.AddHttpClient("OpenAI", client =>
        {
            var url = builder.Configuration.GetValue<string>("OpenAI:BaseUrl");
            client.BaseAddress = new Uri(url ?? throw new ArgumentException("OpenAI BaseUrl is not configured."));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}");
        });

        builder.Services.AddScoped<IModelService, OpenAIService>();

        builder.Services.AddSingleton<Application>();

        var host = builder.Build();

        var app = host.Services.GetRequiredService<Application>();

        return await app.RunAsync(args);
    }
}
