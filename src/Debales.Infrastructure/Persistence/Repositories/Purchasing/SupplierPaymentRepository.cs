using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Purchasing;

internal sealed class SupplierPaymentRepository : BaseRepository<SupplierPayment>, ISupplierPaymentRepository
{
    public SupplierPaymentRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SupplierPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Payable)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<PagedResult<SupplierPayment>> SearchAsync(
        string? search, Guid? supplierId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(p => p.Supplier)
            .Include(p => p.Payable)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Number.ToLower().Contains(term) ||
                p.Supplier!.Name.ToLower().Contains(term));
        }

        if (supplierId.HasValue) query = query.Where(p => p.SupplierId == supplierId);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SupplierPayment>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"PAG-{year}-";
        var count = await Context.Set<SupplierPayment>()
            .IgnoreQueryFilters()
            .CountAsync(p => p.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
