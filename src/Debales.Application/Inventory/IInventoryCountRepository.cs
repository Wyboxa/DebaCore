using Debales.Application.Common;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory;

public interface IInventoryCountRepository : IRepository<InventoryCount>
{
    new Task<InventoryCount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<InventoryCount>> SearchAsync(Guid? warehouseId, InventoryCountStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
