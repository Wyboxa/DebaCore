using Debales.Application.Common;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory;

public interface IStockBalanceRepository : IRepository<StockBalance>
{
    Task<StockBalance?> GetAsync(Guid itemId, Guid warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockBalance>> GetByItemAsync(Guid itemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockBalance>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StockBalance>> GetAllWithStockAsync(CancellationToken cancellationToken = default);
}
