using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Purchasing;

internal sealed class PayableRepository : BaseRepository<Payable>, IPayableRepository
{
    public PayableRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Payable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(p => p.PurchaseInvoice)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Payable>> GetByInvoiceAsync(Guid purchaseInvoiceId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(p => p.Supplier)
            .Where(p => p.PurchaseInvoiceId == purchaseInvoiceId)
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Payable>> SearchAsync(
        string? search, Guid? supplierId, PayableStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(p => p.PurchaseInvoice)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Number.ToLower().Contains(term) ||
                p.Supplier!.Name.ToLower().Contains(term) ||
                p.PurchaseInvoice!.Number.ToLower().Contains(term));
        }

        if (supplierId.HasValue) query = query.Where(p => p.SupplierId == supplierId);
        if (status.HasValue) query = query.Where(p => p.Status == status);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.DueDate)
            .ThenByDescending(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Payable>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"VTP-{year}-";
        var count = await Context.Set<Payable>()
            .IgnoreQueryFilters()
            .CountAsync(p => p.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
