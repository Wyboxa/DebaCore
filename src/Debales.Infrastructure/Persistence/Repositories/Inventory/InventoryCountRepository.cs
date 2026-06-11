using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Inventory;

internal sealed class InventoryCountRepository : BaseRepository<InventoryCount>, IInventoryCountRepository
{
    public InventoryCountRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<InventoryCount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Warehouse)
            .Include(c => c.Lines)
                .ThenInclude(l => l.Item)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<PagedResult<InventoryCount>> SearchAsync(
        Guid? warehouseId, InventoryCountStatus? status, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(c => c.Warehouse)
            .Include(c => c.Lines)
            .AsQueryable();

        if (warehouseId.HasValue) query = query.Where(c => c.WarehouseId == warehouseId);
        if (status.HasValue) query = query.Where(c => c.Status == status);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<InventoryCount>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(c => c.Number == number, cancellationToken);

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"REC-{year}-";
        var count = await Context.Set<InventoryCount>()
            .IgnoreQueryFilters()
            .CountAsync(c => c.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
