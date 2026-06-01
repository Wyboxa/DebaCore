using Debales.Application.Inventory;
using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Inventory;

internal sealed class StockBalanceRepository : BaseRepository<StockBalance>, IStockBalanceRepository
{
    public StockBalanceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<StockBalance?> GetAsync(Guid itemId, Guid warehouseId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(b => b.Item)
            .Include(b => b.Warehouse)
            .FirstOrDefaultAsync(b => b.ItemId == itemId && b.WarehouseId == warehouseId, cancellationToken);

    public async Task<IReadOnlyList<StockBalance>> GetByItemAsync(Guid itemId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(b => b.Item)
            .Include(b => b.Warehouse)
            .Where(b => b.ItemId == itemId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<StockBalance>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(b => b.Item)
            .Include(b => b.Warehouse)
            .Where(b => b.WarehouseId == warehouseId)
            .OrderBy(b => b.Item!.Code)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<StockBalance>> GetAllWithStockAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(b => b.Item)
            .Include(b => b.Warehouse)
            .Where(b => b.Quantity != 0)
            .OrderBy(b => b.Warehouse!.Name)
            .ThenBy(b => b.Item!.Code)
            .ToListAsync(cancellationToken);
}
