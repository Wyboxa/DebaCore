using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface ISupplierItemCodeRepository : IRepository<SupplierItemCode>
{
    Task<List<SupplierItemCode>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<SupplierItemCode?> GetBySupplierAndItemAsync(Guid supplierId, Guid itemId, CancellationToken cancellationToken = default);
}
