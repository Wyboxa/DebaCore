using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IAccountingJournalRepository : IRepository<AccountingJournal>
{
    Task<IReadOnlyList<AccountingJournal>> GetActiveAsync(CancellationToken ct = default);
    Task<AccountingJournal?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
}
