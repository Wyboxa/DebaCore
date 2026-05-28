using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface IUnitOfMeasureRepository : IRepository<UnitOfMeasure>
{
    Task<List<UnitOfMeasure>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
