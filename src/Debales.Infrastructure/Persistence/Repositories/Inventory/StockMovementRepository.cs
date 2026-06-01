using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Inventory;

internal sealed class StockMovementRepository : BaseRepository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(m => m.Warehouse)
            .Include(m => m.Location)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<PagedResult<StockMovement>> SearchAsync(
        string? search, Guid? itemId, Guid? warehouseId, StockMovementType? type,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(m => m.Warehouse)
            .Include(m => m.Location)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(m =>
                m.Number.ToLower().Contains(term) ||
                m.ItemCode.ToLower().Contains(term) ||
                m.ItemName.ToLower().Contains(term));
        }

        if (itemId.HasValue) query = query.Where(m => m.ItemId == itemId);
        if (warehouseId.HasValue) query = query.Where(m => m.WarehouseId == warehouseId);
        if (type.HasValue) query = query.Where(m => m.Type == type);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(m => m.Date)
            .ThenByDescending(m => m.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<StockMovement>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"MOV-{year}-";
        var count = await Context.Set<StockMovement>()
            .IgnoreQueryFilters()
            .CountAsync(m => m.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
