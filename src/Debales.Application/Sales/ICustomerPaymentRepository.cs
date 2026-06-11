using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ICustomerPaymentRepository : IRepository<CustomerPayment>
{
    new Task<CustomerPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<CustomerPayment>> SearchAsync(string? search, Guid? customerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerPayment>> GetByCustomerForStatementAsync(Guid customerId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default);
}
