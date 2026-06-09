using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ISalesOrderRepository : IRepository<SalesOrder>
{
    new Task<SalesOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SalesOrder>> SearchAsync(string? search, Guid? customerId, SalesOrderStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SalesOrder>> GetConfirmedPendingDeliveryAsync(CancellationToken cancellationToken = default);
}
