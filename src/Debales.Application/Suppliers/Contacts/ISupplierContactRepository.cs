using Debales.Application.Common;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Contacts;

public interface ISupplierContactRepository : IRepository<SupplierContact>
{
    Task<IReadOnlyList<SupplierContact>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
}
