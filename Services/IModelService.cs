namespace Sentinel.Services;

public interface IModelService
{
    Task<string?> InvokeAsync(string prompt, CancellationToken cancellationToken);
}
