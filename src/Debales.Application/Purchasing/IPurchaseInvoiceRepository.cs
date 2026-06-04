using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface IPurchaseInvoiceRepository : IRepository<PurchaseInvoice>
{
    new Task<PurchaseInvoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseInvoice>> SearchAsync(string? search, Guid? supplierId, PurchaseInvoiceStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
    Task<PurchaseInvoice?> GetByPurchaseDeliveryNoteIdAsync(Guid purchaseDeliveryNoteId, CancellationToken cancellationToken = default);
}
