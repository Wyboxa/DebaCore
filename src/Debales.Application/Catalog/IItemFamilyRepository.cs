using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface IItemFamilyRepository : IRepository<ItemFamily>
{
    Task<List<ItemFamily>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
