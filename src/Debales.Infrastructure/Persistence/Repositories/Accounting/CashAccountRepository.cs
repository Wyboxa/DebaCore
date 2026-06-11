using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class CashAccountRepository : BaseRepository<CashAccount>, ICashAccountRepository
{
    public CashAccountRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<CashAccount?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet
            .Include(a => a.Account)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<PagedResult<CashAccount>> SearchAsync(
        string? search, bool? isActive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(a =>
                a.Code.ToLower().Contains(term) ||
                a.Name.ToLower().Contains(term));
        }

        if (isActive.HasValue) query = query.Where(a => a.IsActive == isActive);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(a => a.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<CashAccount>(items, total, page, pageSize);
    }

    public async Task<List<CashAccount>> GetAllActiveAsync(CancellationToken ct = default) =>
        await DbSet.Where(a => a.IsActive).OrderBy(a => a.Code).ToListAsync(ct);

    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId, CancellationToken ct = default) =>
        await DbSet.AnyAsync(a => a.Code == code.ToUpperInvariant() && (!excludeId.HasValue || a.Id != excludeId), ct);
}
