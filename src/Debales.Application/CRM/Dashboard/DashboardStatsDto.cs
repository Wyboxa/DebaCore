namespace Debales.Application.CRM.Dashboard;

public sealed record DashboardStatsDto(
    int TotalCustomers,
    int ActiveCustomers,
    int OpenOpportunities,
    decimal PipelineValue,
    int PendingActivities,
    int OverdueActivities,
    IReadOnlyList<RecentCustomerItem> RecentCustomers,
    IReadOnlyList<UpcomingActivityItem> UpcomingActivities,
    IReadOnlyList<PipelineStageItem> Pipeline);

public sealed record RecentCustomerItem(Guid Id, string Name, string? Sector, DateTime CreatedAt);

public sealed record UpcomingActivityItem(
    Guid CustomerId,
    string CustomerName,
    string Type,
    string Subject,
    DateTime ScheduledAt,
    bool IsOverdue);

public sealed record PipelineStageItem(string Stage, int Count, decimal Value);
