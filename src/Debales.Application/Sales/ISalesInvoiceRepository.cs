using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ISalesInvoiceRepository : IRepository<SalesInvoice>
{
    new Task<SalesInvoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SalesInvoice>> SearchAsync(string? search, Guid? customerId, SalesInvoiceStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
    Task<SalesInvoice?> GetBySalesDeliveryNoteIdAsync(Guid salesDeliveryNoteId, CancellationToken cancellationToken = default);
}
