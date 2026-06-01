using Debales.Application.Common;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory;

public interface IStockMovementRepository : IRepository<StockMovement>
{
    new Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<StockMovement>> SearchAsync(string? search, Guid? itemId, Guid? warehouseId, StockMovementType? type, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
