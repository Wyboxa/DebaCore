using Debales.Application.Common;
using Debales.Application.Core.Roles;

namespace Debales.Application.Core.Users.Commands.AssignRole;

public sealed class AssignRoleHandler
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IUnitOfWork _uow;

    public AssignRoleHandler(IUserRepository users, IRoleRepository roles, IUnitOfWork uow)
    {
        _users = users;
        _roles = roles;
        _uow = uow;
    }

    public async Task Handle(AssignRoleCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetAllWithRolesAsync(cancellationToken)
            .ContinueWith(t => t.Result.FirstOrDefault(u => u.Id == command.UserId));

        if (user is null) throw new InvalidOperationException("Usuario no encontrado.");

        var role = await _roles.GetByIdAsync(command.RoleId, cancellationToken)
            ?? throw new InvalidOperationException("Rol no encontrado.");

        // Limpia roles existentes y asigna el nuevo (un único rol por usuario en MVP)
        foreach (var ur in user.UserRoles.ToList())
            user.RemoveRole(ur.RoleId);

        user.AssignRole(role);
        _users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
