using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface IPriceListRepository : IRepository<PriceList>
{
    new Task<PriceList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<PriceList>> SearchAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<PriceList>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
