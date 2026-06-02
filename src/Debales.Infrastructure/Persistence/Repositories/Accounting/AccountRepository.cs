using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _db;

    public AccountRepository(ApplicationDbContext db) => _db = db;

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Accounts.OrderBy(a => a.Code).ToListAsync(ct);

    public async Task<PagedResult<Account>> SearchAsync(string? search, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.Accounts.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Code.Contains(search) || a.Name.Contains(search));

        query = query.OrderBy(a => a.Code);
        var total = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedResult<Account>(items, total, page, pageSize);
    }

    public async Task<Account?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await _db.Accounts.FirstOrDefaultAsync(a => a.Code == code, ct);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default) =>
        await _db.Accounts.AnyAsync(a => a.Code == code, ct);

    public async Task AddAsync(Account entity, CancellationToken ct = default) =>
        await _db.Accounts.AddAsync(entity, ct);

    public void Update(Account entity) => _db.Accounts.Update(entity);

    public void Remove(Account entity) => _db.Accounts.Remove(entity);
}
