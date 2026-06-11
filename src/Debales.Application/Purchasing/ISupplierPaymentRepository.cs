using Debales.Application.Common;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing;

public interface ISupplierPaymentRepository : IRepository<SupplierPayment>
{
    new Task<SupplierPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SupplierPayment>> SearchAsync(string? search, Guid? supplierId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupplierPayment>> GetBySupplierForStatementAsync(Guid supplierId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default);
}
