using Debales.Domain.Common;

namespace Debales.Domain.Core.Roles;

public sealed class Role : Entity
{
    private readonly List<RolePermission> _rolePermissions = [];

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsSystem { get; private set; }

    public IReadOnlyList<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Role() { }

    public static Role Create(string name, string description, string createdBy, bool isSystem = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del rol no puede estar vacío.", nameof(name));

        return new Role
        {
            Name = name.Trim(),
            Description = description.Trim(),
            IsSystem = isSystem,
            CreatedBy = createdBy
        };
    }

    public void AddPermission(Permission permission)
    {
        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            return;

        _rolePermissions.Add(new RolePermission(Id, permission.Id));
    }
}
