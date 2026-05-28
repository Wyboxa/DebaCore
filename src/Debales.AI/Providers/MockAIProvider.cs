namespace Debales.AI.Providers;

public sealed class MockAIProvider : IAIProvider
{
    public string Name => "Mock";

    public Task<string> ChatAsync(string systemPrompt, IReadOnlyList<(string Role, string Content)> messages, CancellationToken ct = default)
    {
        var last = messages.LastOrDefault();
        return Task.FromResult(
            $"[Mock] Mensaje recibido: \"{last.Content}\". Sistema activo con contexto de {messages.Count} mensaje(s).");
    }
}
