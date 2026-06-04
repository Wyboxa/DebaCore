using Debales.Application.Core.Roles;
using Debales.Domain.Core.Roles;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
}
