using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Purchasing;

internal sealed class PurchaseInvoiceRepository : BaseRepository<PurchaseInvoice>, IPurchaseInvoiceRepository
{
    public PurchaseInvoiceRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PurchaseInvoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(i => i.Supplier)
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<PagedResult<PurchaseInvoice>> SearchAsync(
        string? search, Guid? supplierId, PurchaseInvoiceStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(i => i.Supplier).Include(i => i.Lines).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(i =>
                i.Number.ToLower().Contains(term) ||
                i.Supplier!.Name.ToLower().Contains(term));
        }

        if (supplierId.HasValue) query = query.Where(i => i.SupplierId == supplierId);
        if (status.HasValue) query = query.Where(i => i.Status == status);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(i => i.Date)
            .ThenByDescending(i => i.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PurchaseInvoice>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(i => i.Number == number.Trim().ToUpper(), cancellationToken);

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"FC-{year}-";
        var count = await Context.Set<PurchaseInvoice>()
            .IgnoreQueryFilters()
            .CountAsync(i => i.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
