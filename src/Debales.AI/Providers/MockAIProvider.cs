namespace Debales.AI.Providers;

public sealed class MockAIProvider : IAIProvider
{
    public string Name => "Mock";

    public Task<string> CompleteAsync(string prompt, CancellationToken cancellationToken = default) =>
        Task.FromResult($"[MockAI] Prompt recibido: {prompt}");
}
