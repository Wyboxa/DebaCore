using Debales.Application.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Application.CRM.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Customer>> SearchAsync(string? search, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
}
