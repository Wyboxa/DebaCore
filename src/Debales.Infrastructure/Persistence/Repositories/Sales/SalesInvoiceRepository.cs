using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class SalesInvoiceRepository : BaseRepository<SalesInvoice>, ISalesInvoiceRepository
{
    public SalesInvoiceRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SalesInvoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(i => i.Customer)
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<PagedResult<SalesInvoice>> SearchAsync(
        string? search, Guid? customerId, SalesInvoiceStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(i => i.Customer).Include(i => i.Lines).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(i =>
                i.Number.ToLower().Contains(term) ||
                i.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue) query = query.Where(i => i.CustomerId == customerId);
        if (status.HasValue) query = query.Where(i => i.Status == status);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(i => i.Date)
            .ThenByDescending(i => i.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SalesInvoice>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(i => i.Number == number.Trim().ToUpper(), cancellationToken);

    public async Task<SalesInvoice?> GetBySalesDeliveryNoteIdAsync(Guid salesDeliveryNoteId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(i => i.Customer)
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.SalesDeliveryNoteId == salesDeliveryNoteId, cancellationToken);
}
