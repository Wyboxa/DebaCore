using Debales.Application.CRM.Dashboard;

namespace Debales.Application.AI.Briefing;

public sealed record GetDashboardBriefingQuery(DashboardStatsDto Stats);

public sealed class GetDashboardBriefingHandler
{
    private readonly IAIService _ai;

    public GetDashboardBriefingHandler(IAIService ai) => _ai = ai;

    public Task<string> Handle(GetDashboardBriefingQuery query, CancellationToken ct = default) =>
        _ai.GetDashboardBriefingAsync(query.Stats, ct);
}
