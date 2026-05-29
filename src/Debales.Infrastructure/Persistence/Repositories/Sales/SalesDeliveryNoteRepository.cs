using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class SalesDeliveryNoteRepository : BaseRepository<SalesDeliveryNote>, ISalesDeliveryNoteRepository
{
    public SalesDeliveryNoteRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SalesDeliveryNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(n => n.Customer)
            .Include(n => n.SalesOrder)
            .Include(n => n.Lines)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<PagedResult<SalesDeliveryNote>> SearchAsync(
        string? search, Guid? customerId, SalesDeliveryNoteStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(n => n.Customer)
            .Include(n => n.SalesOrder)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(n =>
                n.Number.ToLower().Contains(term) ||
                n.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue)
            query = query.Where(n => n.CustomerId == customerId);

        if (status.HasValue)
            query = query.Where(n => n.Status == status);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(n => n.Date)
            .ThenByDescending(n => n.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SalesDeliveryNote>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"ALV-{year}-";
        var count = await Context.Set<SalesDeliveryNote>()
            .IgnoreQueryFilters()
            .CountAsync(n => n.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
