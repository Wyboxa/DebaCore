using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface IPurchaseCreditNoteRepository : IRepository<PurchaseCreditNote>
{
    new Task<PurchaseCreditNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PurchaseCreditNote>> SearchAsync(string? search, Guid? supplierId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseCreditNote>> GetBySupplierForStatementAsync(Guid supplierId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default);
}
