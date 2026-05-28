namespace Debales.AI.Providers;

public interface IAIProvider
{
    string Name { get; }
    Task<string> ChatAsync(string systemPrompt, IReadOnlyList<(string Role, string Content)> messages, CancellationToken ct = default);
}
