namespace Debales.Application.AI;

public interface IAIService
{
    Task<string> ChatAsync(CustomerAIContext context, IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default);
    Task<string> GetCustomerSummaryAsync(CustomerAIContext context, CancellationToken ct = default);
    Task<string> GetDashboardBriefingAsync(CRM.Dashboard.DashboardStatsDto stats, CancellationToken ct = default);
}
