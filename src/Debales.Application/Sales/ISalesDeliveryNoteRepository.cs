using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ISalesDeliveryNoteRepository : IRepository<SalesDeliveryNote>
{
    new Task<SalesDeliveryNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SalesDeliveryNote>> SearchAsync(string? search, Guid? customerId, SalesDeliveryNoteStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SalesDeliveryNote>> GetBySalesOrderIdAsync(Guid salesOrderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SalesDeliveryNote>> GetPostedWithoutInvoiceAsync(CancellationToken cancellationToken = default);
}
