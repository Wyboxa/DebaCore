using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface IPayableRepository : IRepository<Payable>
{
    new Task<Payable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payable>> GetByInvoiceAsync(Guid purchaseInvoiceId, CancellationToken cancellationToken = default);
    Task<PagedResult<Payable>> SearchAsync(string? search, Guid? supplierId, PayableStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payable>> GetForAgingAsync(Guid? supplierId = null, CancellationToken ct = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
