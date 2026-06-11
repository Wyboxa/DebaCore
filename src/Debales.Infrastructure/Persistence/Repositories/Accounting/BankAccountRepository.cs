using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
{
    public BankAccountRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<PagedResult<BankAccount>> SearchAsync(
        string? search, bool? isActive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(b =>
                b.Name.ToLower().Contains(term) ||
                (b.BankName != null && b.BankName.ToLower().Contains(term)) ||
                (b.Iban != null && b.Iban.ToLower().Contains(term)));
        }

        if (isActive.HasValue) query = query.Where(b => b.IsActive == isActive);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(b => b.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<BankAccount>(items, total, page, pageSize);
    }

    public async Task<List<BankAccount>> GetAllActiveAsync(CancellationToken ct = default) =>
        await DbSet.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync(ct);
}
