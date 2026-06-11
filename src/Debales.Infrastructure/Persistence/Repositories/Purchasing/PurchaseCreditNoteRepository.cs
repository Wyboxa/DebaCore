using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Purchasing;

internal sealed class PurchaseCreditNoteRepository : BaseRepository<PurchaseCreditNote>, IPurchaseCreditNoteRepository
{
    public PurchaseCreditNoteRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PurchaseCreditNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(n => n.Supplier)
            .Include(n => n.OriginalInvoice)
            .Include(n => n.Lines)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<PagedResult<PurchaseCreditNote>> SearchAsync(
        string? search, Guid? supplierId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(n => n.Supplier)
            .Include(n => n.OriginalInvoice)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(n =>
                n.Number.ToLower().Contains(term) ||
                n.Supplier!.Name.ToLower().Contains(term));
        }

        if (supplierId.HasValue) query = query.Where(n => n.SupplierId == supplierId);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PurchaseCreditNote>(items, total, page, pageSize);
    }

    public async Task<IReadOnlyList<PurchaseCreditNote>> GetBySupplierForStatementAsync(
        Guid supplierId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(n => n.Lines).Where(n => n.SupplierId == supplierId);
        if (from.HasValue) query = query.Where(n => n.Date >= from.Value);
        if (to.HasValue) query = query.Where(n => n.Date <= to.Value);
        return await query.OrderBy(n => n.Date).ThenBy(n => n.Number).ToListAsync(cancellationToken);
    }
}
