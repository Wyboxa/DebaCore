using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface ITaxTypeRepository : IRepository<TaxType>
{
    Task<List<TaxType>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
