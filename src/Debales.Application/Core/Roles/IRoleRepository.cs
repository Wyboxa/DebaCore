using Debales.Application.Common;
using Debales.Domain.Core.Roles;

namespace Debales.Application.Core.Roles;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
