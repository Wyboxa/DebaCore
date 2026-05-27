using Debales.Domain.Core.Roles;

namespace Debales.Domain.Tests.Core.Roles;

public sealed class RoleTests
{
    [Fact]
    public void Role_Create_WithValidData_Succeeds()
    {
        var role = Role.Create("Admin", "Administrador del sistema", "system", isSystem: true);

        Assert.Equal("Admin", role.Name);
        Assert.True(role.IsSystem);
        Assert.Empty(role.RolePermissions);
    }

    [Fact]
    public void Role_AddPermission_AddsOnce()
    {
        var role = Role.Create("Admin", "Administrador", "system");
        var perm = Permission.Create("core.users.read", "Leer usuarios", "system");

        role.AddPermission(perm);
        role.AddPermission(perm);

        Assert.Single(role.RolePermissions);
    }

    [Fact]
    public void Permission_Create_NormalizesKeyToLowercase()
    {
        var perm = Permission.Create("CORE.USERS.READ", "Leer usuarios", "system");

        Assert.Equal("core.users.read", perm.Key);
    }
}
