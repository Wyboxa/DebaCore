using Debales.Application.Common;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    new Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Warehouse>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
