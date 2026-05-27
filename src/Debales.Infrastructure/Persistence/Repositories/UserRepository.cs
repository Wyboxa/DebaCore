using Debales.Application.Core.Users;
using Debales.Domain.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(u => u.Username == username, cancellationToken);
}
