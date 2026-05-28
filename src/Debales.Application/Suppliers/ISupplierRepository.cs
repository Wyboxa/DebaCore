using Debales.Application.Common;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers;

public interface ISupplierRepository : IRepository<Supplier>
{
    new Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Supplier>> SearchAsync(string? search, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
}
