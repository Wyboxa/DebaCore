using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class AccountingJournalRepository : IAccountingJournalRepository
{
    private readonly ApplicationDbContext _db;

    public AccountingJournalRepository(ApplicationDbContext db) => _db = db;

    public async Task<AccountingJournal?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.AccountingJournals.FirstOrDefaultAsync(j => j.Id == id, ct);

    public async Task<IReadOnlyList<AccountingJournal>> GetAllAsync(CancellationToken ct = default) =>
        await _db.AccountingJournals.OrderBy(j => j.Code).ToListAsync(ct);

    public async Task<IReadOnlyList<AccountingJournal>> GetActiveAsync(CancellationToken ct = default) =>
        await _db.AccountingJournals.Where(j => j.IsActive).OrderBy(j => j.Code).ToListAsync(ct);

    public async Task<AccountingJournal?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await _db.AccountingJournals.FirstOrDefaultAsync(j => j.Code == code, ct);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        await _db.AccountingJournals.AnyAsync(j => j.Code == code, ct);

    public async Task AddAsync(AccountingJournal entity, CancellationToken ct = default) =>
        await _db.AccountingJournals.AddAsync(entity, ct);

    public void Update(AccountingJournal entity) => _db.AccountingJournals.Update(entity);

    public void Remove(AccountingJournal entity) => _db.AccountingJournals.Remove(entity);
}
