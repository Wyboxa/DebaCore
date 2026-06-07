using Debales.Application.Core.Audit.DTOs;

namespace Debales.Application.Core.Audit.Queries.GetAuditEntries;

public sealed class GetAuditEntriesHandler
{
    private readonly IAuditEntryRepository _repo;

    public GetAuditEntriesHandler(IAuditEntryRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<AuditEntrySummaryDto>> Handle(
        GetAuditEntriesQuery query,
        CancellationToken ct = default)
    {
        var entries = await _repo.GetRecentAsync(query.EntityName, query.From, query.To, query.Take, ct);

        return entries
            .Select(e => new AuditEntrySummaryDto(
                e.Id, e.EntityName, e.EntityId, e.Action,
                e.UserId, e.OldValues, e.NewValues, e.CreatedAt))
            .ToList();
    }
}
