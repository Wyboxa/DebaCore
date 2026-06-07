using Debales.Domain.Core.Audit;

namespace Debales.Application.Core.Audit;

public interface IAuditEntryRepository
{
    Task<IReadOnlyList<AuditEntry>> GetRecentAsync(
        string? entityName,
        DateTime? from,
        DateTime? to,
        int take,
        CancellationToken ct = default);

    Task<IReadOnlyList<string>> GetDistinctEntityNamesAsync(CancellationToken ct = default);
}
