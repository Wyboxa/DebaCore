using Debales.Application.CRM.Activities;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Opportunities;
using Debales.Domain.CRM.Opportunities;

namespace Debales.Application.CRM.Dashboard;

public sealed record GetDashboardStatsQuery;

public sealed class GetDashboardStatsHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IOpportunityRepository _opportunities;
    private readonly IActivityRepository _activities;

    public GetDashboardStatsHandler(
        ICustomerRepository customers,
        IOpportunityRepository opportunities,
        IActivityRepository activities)
    {
        _customers = customers;
        _opportunities = opportunities;
        _activities = activities;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery _, CancellationToken ct = default)
    {
        var allCustomers = await _customers.GetAllAsync(ct);
        var allOpportunities = await _opportunities.GetAllAsync(ct);
        var allActivities = await _activities.GetAllAsync(ct);

        var now = DateTime.UtcNow;
        var horizon = now.AddDays(14);

        // ── Clientes ──────────────────────────────────────────────────────────
        var activeCustomers = allCustomers.Where(c => c.IsActive).ToList();
        var recentCustomers = allCustomers
            .OrderByDescending(c => c.CreatedAt)
            .Take(6)
            .Select(c => new RecentCustomerItem(c.Id, c.Name, c.Sector, c.CreatedAt))
            .ToList();

        // ── Oportunidades ─────────────────────────────────────────────────────
        var openOpps = allOpportunities
            .Where(o => o.Status is OpportunityStatus.New or OpportunityStatus.InProgress)
            .ToList();
        var pipelineValue = openOpps.Sum(o => o.EstimatedValue ?? 0m);

        var pipeline = allOpportunities
            .GroupBy(o => o.Status.ToString())
            .Select(g => new PipelineStageItem(
                g.Key,
                g.Count(),
                g.Sum(o => o.EstimatedValue ?? 0m)))
            .ToList();

        // ── Actividades ───────────────────────────────────────────────────────
        var customerById = allCustomers.ToDictionary(c => c.Id, c => c.Name);
        var pendingActivities = allActivities.Where(a => !a.IsCompleted).ToList();

        var overdue = pendingActivities
            .Where(a => a.ScheduledAt < now)
            .ToList();

        var upcoming = pendingActivities
            .Where(a => a.ScheduledAt >= now && a.ScheduledAt <= horizon)
            .OrderBy(a => a.ScheduledAt)
            .Take(8)
            .Select(a => new UpcomingActivityItem(
                a.CustomerId,
                customerById.TryGetValue(a.CustomerId, out var name) ? name : "—",
                a.Type.ToString(),
                a.Subject,
                a.ScheduledAt,
                false))
            .Concat(overdue
                .OrderByDescending(a => a.ScheduledAt)
                .Take(3)
                .Select(a => new UpcomingActivityItem(
                    a.CustomerId,
                    customerById.TryGetValue(a.CustomerId, out var name) ? name : "—",
                    a.Type.ToString(),
                    a.Subject,
                    a.ScheduledAt,
                    true)))
            .OrderBy(a => a.IsOverdue ? 0 : 1)
            .ThenBy(a => a.ScheduledAt)
            .Take(8)
            .ToList();

        return new DashboardStatsDto(
            allCustomers.Count,
            activeCustomers.Count,
            openOpps.Count,
            pipelineValue,
            pendingActivities.Count,
            overdue.Count,
            recentCustomers,
            upcoming,
            pipeline);
    }
}

