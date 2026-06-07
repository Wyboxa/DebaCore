using Debales.Application.Common;
using Debales.Application.Core.Roles;
using Debales.Application.Core.Users;
using Debales.Application.Core.Users.Commands.AssignRole;
using Debales.Domain.Core.Roles;
using Debales.Domain.Core.Users;
using NSubstitute;

namespace Debales.Application.Tests.Core.Users;

public sealed class AssignRoleHandlerTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly IRoleRepository _roles = Substitute.For<IRoleRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly AssignRoleHandler _handler;

    private static User MakeUser()
    {
        var email = Email.Create("test@debales.com");
        return User.Create("testuser", email, "hash", "system");
    }

    private static Role MakeRole() =>
        Role.Create("Admin", "Administrador", "system");

    public AssignRoleHandlerTests()
    {
        _handler = new AssignRoleHandler(_users, _roles, _uow);
    }

    [Fact]
    public async Task Handle_AssignsRoleToUser()
    {
        var user = MakeUser();
        var role = MakeRole();
        _users.GetAllWithRolesAsync(Arg.Any<CancellationToken>()).Returns([user]);
        _roles.GetByIdAsync(role.Id, Arg.Any<CancellationToken>()).Returns(role);

        await _handler.Handle(new AssignRoleCommand(user.Id, role.Id, "system"));

        Assert.Single(user.UserRoles);
        Assert.Equal(role.Id, user.UserRoles[0].RoleId);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ReplacesExistingRole()
    {
        var user = MakeUser();
        var oldRole = Role.Create("Viewer", "Sólo lectura", "system");
        var newRole = MakeRole();
        user.AssignRole(oldRole);

        _users.GetAllWithRolesAsync(Arg.Any<CancellationToken>()).Returns([user]);
        _roles.GetByIdAsync(newRole.Id, Arg.Any<CancellationToken>()).Returns(newRole);

        await _handler.Handle(new AssignRoleCommand(user.Id, newRole.Id, "system"));

        Assert.Single(user.UserRoles);
        Assert.Equal(newRole.Id, user.UserRoles[0].RoleId);
    }

    [Fact]
    public async Task Handle_WithUnknownUser_Throws()
    {
        _users.GetAllWithRolesAsync(Arg.Any<CancellationToken>()).Returns([]);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new AssignRoleCommand(Guid.NewGuid(), Guid.NewGuid(), "system")));
    }

    [Fact]
    public async Task Handle_WithUnknownRole_Throws()
    {
        var user = MakeUser();
        _users.GetAllWithRolesAsync(Arg.Any<CancellationToken>()).Returns([user]);
        _roles.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Role?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new AssignRoleCommand(user.Id, Guid.NewGuid(), "system")));
    }
}
