using Debales.Application.AI.ERP;

namespace Debales.Application.AI;

public interface IAIService
{
    // CRM
    Task<string> ChatAsync(CustomerAIContext context, IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default);
    Task<string> GetCustomerSummaryAsync(CustomerAIContext context, CancellationToken ct = default);
    Task<string> GetDashboardBriefingAsync(CRM.Dashboard.DashboardStatsDto stats, CancellationToken ct = default);

    // ERP-6
    Task<string> ChatWithERPAsync(ERPAIContext context, IReadOnlyList<ChatMessage> history, string userMessage, CancellationToken ct = default);
    Task<string> GetCustomerERPSummaryAsync(CustomerERPContext context, CancellationToken ct = default);
    Task<string> GetSupplierERPSummaryAsync(SupplierAIContext context, CancellationToken ct = default);
}
