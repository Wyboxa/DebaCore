namespace Debales.AI.Providers;

public interface IAIProvider
{
    string Name { get; }
    Task<string> CompleteAsync(string prompt, CancellationToken cancellationToken = default);
}
