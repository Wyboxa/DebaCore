using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface IPaymentTermRepository : IRepository<PaymentTerm>
{
    new Task<PaymentTerm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PaymentTerm>> SearchAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<PaymentTerm>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
