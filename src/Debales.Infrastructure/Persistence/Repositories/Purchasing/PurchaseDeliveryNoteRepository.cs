using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Purchasing;

internal sealed class PurchaseDeliveryNoteRepository : BaseRepository<PurchaseDeliveryNote>, IPurchaseDeliveryNoteRepository
{
    public PurchaseDeliveryNoteRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PurchaseDeliveryNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(n => n.Supplier)
            .Include(n => n.PurchaseOrder)
            .Include(n => n.Lines)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<PagedResult<PurchaseDeliveryNote>> SearchAsync(
        string? search, Guid? supplierId, PurchaseDeliveryNoteStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(n => n.Supplier)
            .Include(n => n.PurchaseOrder)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(n =>
                n.Number.ToLower().Contains(term) ||
                n.Supplier!.Name.ToLower().Contains(term));
        }

        if (supplierId.HasValue)
            query = query.Where(n => n.SupplierId == supplierId);

        if (status.HasValue)
            query = query.Where(n => n.Status == status);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(n => n.Date)
            .ThenByDescending(n => n.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PurchaseDeliveryNote>(items, total, page, pageSize);
    }

}
