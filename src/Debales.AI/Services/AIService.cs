using Debales.Application.AI;
using Debales.Application.CRM.Dashboard;
using Debales.AI.Providers;

namespace Debales.AI.Services;

public sealed class AIService : IAIService
{
    private readonly IAIProvider _provider;

    public AIService(IAIProvider provider) => _provider = provider;

    public async Task<string> ChatAsync(CustomerAIContext context, IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default)
    {
        var systemPrompt = PromptBuilder.BuildSystemPrompt(context);

        var messages = history
            .Select(m => (m.Role, m.Content))
            .Append(("user", userMessage))
            .ToList();

        return await _provider.ChatAsync(systemPrompt, messages, ct);
    }

    public async Task<string> GetCustomerSummaryAsync(CustomerAIContext context, CancellationToken ct = default)
    {
        var systemPrompt = PromptBuilder.BuildSystemPrompt(context);
        var messages = new List<(string Role, string Content)>
        {
            ("user", PromptBuilder.BuildSummaryPrompt())
        };

        return await _provider.ChatAsync(systemPrompt, messages, ct);
    }

    public async Task<string> GetDashboardBriefingAsync(DashboardStatsDto stats, CancellationToken ct = default)
    {
        var messages = new List<(string Role, string Content)>
        {
            ("user", PromptBuilder.BuildDashboardBriefingMessage(stats))
        };

        return await _provider.ChatAsync(PromptBuilder.BuildDashboardSystemPrompt(), messages, ct);
    }
}
