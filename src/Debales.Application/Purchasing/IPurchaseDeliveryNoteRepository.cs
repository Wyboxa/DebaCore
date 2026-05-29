using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface IPurchaseDeliveryNoteRepository : IRepository<PurchaseDeliveryNote>
{
    new Task<PurchaseDeliveryNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseDeliveryNote>> SearchAsync(string? search, Guid? supplierId, PurchaseDeliveryNoteStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
