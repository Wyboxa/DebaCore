using Debales.Application.Common;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory;

public interface IWarehouseLocationRepository : IRepository<WarehouseLocation>
{
    Task<IReadOnlyList<WarehouseLocation>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(Guid warehouseId, string code, CancellationToken cancellationToken = default);
}
