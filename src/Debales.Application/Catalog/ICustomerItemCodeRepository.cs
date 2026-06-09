using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface ICustomerItemCodeRepository : IRepository<CustomerItemCode>
{
    Task<List<CustomerItemCode>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<CustomerItemCode?> GetByCustomerAndItemAsync(Guid customerId, Guid itemId, CancellationToken cancellationToken = default);
}
