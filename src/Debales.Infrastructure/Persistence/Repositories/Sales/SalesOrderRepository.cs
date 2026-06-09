using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class SalesOrderRepository : BaseRepository<SalesOrder>, ISalesOrderRepository
{
    public SalesOrderRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SalesOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<PagedResult<SalesOrder>> SearchAsync(
        string? search, Guid? customerId, SalesOrderStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(o =>
                o.Number.ToLower().Contains(term) ||
                o.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId);

        if (status.HasValue)
            query = query.Where(o => o.Status == status);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => o.Date)
            .ThenByDescending(o => o.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SalesOrder>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(o => o.Number == number.Trim().ToUpper(), cancellationToken);

    public async Task<IReadOnlyList<SalesOrder>> GetConfirmedPendingDeliveryAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .Where(o => o.Status == SalesOrderStatus.Confirmed || o.Status == SalesOrderStatus.PartiallyDelivered)
            .OrderBy(o => o.Date)
            .ToListAsync(cancellationToken);
}
