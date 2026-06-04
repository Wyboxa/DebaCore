using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class AccountingEntryRepository : IAccountingEntryRepository
{
    private readonly ApplicationDbContext _db;

    public AccountingEntryRepository(ApplicationDbContext db) => _db = db;

    public async Task<AccountingEntry?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.AccountingEntries
            .Include(e => e.Journal)
            .Include(e => e.FiscalPeriod)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<AccountingEntry?> GetByIdWithLinesAsync(Guid id, CancellationToken ct = default) =>
        await _db.AccountingEntries
            .Include(e => e.Journal)
            .Include(e => e.FiscalPeriod)
            .Include(e => e.Lines)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<AccountingEntry>> GetAllAsync(CancellationToken ct = default) =>
        await _db.AccountingEntries
            .Include(e => e.Journal)
            .Include(e => e.FiscalPeriod)
            .OrderByDescending(e => e.Date)
            .ToListAsync(ct);

    public async Task<PagedResult<AccountingEntry>> SearchAsync(
        string? search, Guid? journalId, Guid? fiscalPeriodId,
        int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.AccountingEntries
            .Include(e => e.Journal)
            .Include(e => e.FiscalPeriod)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Number.Contains(search) || e.Description.Contains(search));
        if (journalId.HasValue)
            query = query.Where(e => e.JournalId == journalId.Value);
        if (fiscalPeriodId.HasValue)
            query = query.Where(e => e.FiscalPeriodId == fiscalPeriodId.Value);

        query = query.OrderByDescending(e => e.Date).ThenByDescending(e => e.Number);
        var total = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedResult<AccountingEntry>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(Guid journalId, string journalCode, CancellationToken ct = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"{journalCode}/{year}/";
        var lastNumber = await _db.AccountingEntries
            .IgnoreQueryFilters()
            .Where(e => e.Number.StartsWith(prefix))
            .OrderByDescending(e => e.Number)
            .Select(e => e.Number)
            .FirstOrDefaultAsync(ct);

        var seq = 1;
        if (lastNumber is not null && lastNumber.Length > prefix.Length)
        {
            if (int.TryParse(lastNumber[prefix.Length..], out var last))
                seq = last + 1;
        }

        return $"{prefix}{seq:D5}";
    }

    public async Task<IReadOnlyList<(string AccountId, string AccountCode, string AccountName, string AccountType, decimal TotalDebit, decimal TotalCredit)>>
        GetTrialBalanceDataAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var query = _db.AccountingEntryLines
            .Join(_db.AccountingEntries,
                l => l.AccountingEntryId,
                e => e.Id,
                (l, e) => new { Line = l, Entry = e })
            .Join(_db.Accounts,
                x => x.Line.AccountId,
                a => a.Id,
                (x, a) => new { x.Line, x.Entry, Account = a })
            .Where(x => x.Entry.Status == EntryStatus.Posted);

        if (from.HasValue)
            query = query.Where(x => x.Entry.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(x => x.Entry.Date <= to.Value);

        var grouped = await query
            .GroupBy(x => new
            {
                AccountId = x.Account.Id.ToString(),
                x.Account.Code,
                x.Account.Name,
                AccountType = x.Account.Type.ToString()
            })
            .Select(g => new
            {
                g.Key.AccountId,
                g.Key.Code,
                g.Key.Name,
                g.Key.AccountType,
                TotalDebit = g.Sum(x => x.Line.Debit),
                TotalCredit = g.Sum(x => x.Line.Credit)
            })
            .OrderBy(r => r.Code)
            .ToListAsync(ct);

        return grouped
            .Select(r => (r.AccountId, r.Code, r.Name, r.AccountType, r.TotalDebit, r.TotalCredit))
            .ToList();
    }

    public async Task<IReadOnlyList<AccountingEntry>> GetPostedEntriesWithLinesAsync(
        DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var query = _db.AccountingEntries
            .Include(e => e.Journal)
            .Include(e => e.Lines)
            .Where(e => e.Status == EntryStatus.Posted);

        if (from.HasValue)
            query = query.Where(e => e.Date >= from.Value);
        if (to.HasValue)
            query = query.Where(e => e.Date <= to.Value);

        return await query
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Number)
            .ToListAsync(ct);
    }

    public async Task AddAsync(AccountingEntry entity, CancellationToken ct = default) =>
        await _db.AccountingEntries.AddAsync(entity, ct);

    public void Update(AccountingEntry entity) => _db.AccountingEntries.Update(entity);

    public void Remove(AccountingEntry entity) => _db.AccountingEntries.Remove(entity);
}
