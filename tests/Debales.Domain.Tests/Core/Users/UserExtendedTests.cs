using Debales.Domain.Core.Roles;
using Debales.Domain.Core.Users;

namespace Debales.Domain.Tests.Core.Users;

public sealed class UserExtendedTests
{
    private static User CreateUser() =>
        User.Create("carlos", Email.Create("carlos@debales.com"), "hash", "system");

    [Fact]
    public void Reactivate_AfterDeactivate_SetsIsActiveTrue()
    {
        var user = CreateUser();
        user.Deactivate("admin");
        user.Reactivate("admin");

        Assert.True(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void UpdatePasswordHash_WithValidHash_UpdatesHash()
    {
        var user = CreateUser();
        user.UpdatePasswordHash("newHash123", "admin");

        Assert.Equal("newHash123", user.PasswordHash);
    }

    [Fact]
    public void UpdatePasswordHash_WithEmptyHash_Throws()
    {
        var user = CreateUser();

        Assert.Throws<ArgumentException>(() => user.UpdatePasswordHash("", "admin"));
    }

    [Fact]
    public void AssignRole_NewRole_AppearsInUserRoles()
    {
        var user = CreateUser();
        var role = Role.Create("Admin", "Administrador", "system", isSystem: true);

        user.AssignRole(role);

        Assert.Single(user.UserRoles);
        Assert.Equal(role.Id, user.UserRoles[0].RoleId);
    }

    [Fact]
    public void AssignRole_SameRoleTwice_OnlyOneEntry()
    {
        var user = CreateUser();
        var role = Role.Create("Admin", "Administrador", "system", isSystem: true);

        user.AssignRole(role);
        user.AssignRole(role);

        Assert.Single(user.UserRoles);
    }

    [Fact]
    public void RemoveRole_ExistingRole_RemovesIt()
    {
        var user = CreateUser();
        var role = Role.Create("Admin", "Administrador", "system", isSystem: true);
        user.AssignRole(role);

        user.RemoveRole(role.Id);

        Assert.Empty(user.UserRoles);
    }
}
