using Debales.Application.Core.Audit;
using Debales.Domain.Core.Audit;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories;

public sealed class AuditEntryRepository : IAuditEntryRepository
{
    private readonly ApplicationDbContext _db;

    public AuditEntryRepository(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<AuditEntry>> GetRecentAsync(
        string? entityName,
        DateTime? from,
        DateTime? to,
        int take,
        CancellationToken ct = default)
    {
        var q = _db.AuditEntries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(entityName))
            q = q.Where(e => e.EntityName == entityName);

        if (from.HasValue)
            q = q.Where(e => e.CreatedAt >= from.Value);

        if (to.HasValue)
            q = q.Where(e => e.CreatedAt < to.Value.AddDays(1));

        return await q
            .OrderByDescending(e => e.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<string>> GetDistinctEntityNamesAsync(CancellationToken ct = default)
    {
        return await _db.AuditEntries
            .AsNoTracking()
            .Select(e => e.EntityName)
            .Distinct()
            .OrderBy(n => n)
            .ToListAsync(ct);
    }
}
