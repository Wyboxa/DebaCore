using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
    new Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseOrder>> SearchAsync(string? search, Guid? supplierId, PurchaseOrderStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
