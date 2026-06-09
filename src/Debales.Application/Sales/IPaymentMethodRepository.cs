using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    new Task<PaymentMethod?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PaymentMethod>> SearchAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<PaymentMethod>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
