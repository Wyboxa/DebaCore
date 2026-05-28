using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog;

public interface IItemRepository : IRepository<Item>
{
    new Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Item>> SearchAsync(string? search, Guid? familyId, bool? isService, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, Guid excludeId, CancellationToken cancellationToken = default);
}
