using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IAccountingEntryRepository : IRepository<AccountingEntry>
{
    Task<PagedResult<AccountingEntry>> SearchAsync(
        string? search, Guid? journalId, Guid? fiscalPeriodId,
        int page, int pageSize, CancellationToken ct = default);
    Task<AccountingEntry?> GetByIdWithLinesAsync(Guid id, CancellationToken ct = default);
    Task<string> GetNextNumberAsync(Guid journalId, string journalCode, CancellationToken ct = default);

    // Reports
    Task<IReadOnlyList<(string AccountId, string AccountCode, string AccountName, string AccountType, decimal TotalDebit, decimal TotalCredit)>>
        GetTrialBalanceDataAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default);

    Task<IReadOnlyList<AccountingEntry>> GetPostedEntriesWithLinesAsync(
        DateOnly? from, DateOnly? to, CancellationToken ct = default);
}
