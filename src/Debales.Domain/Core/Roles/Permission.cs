using Debales.Domain.Common;

namespace Debales.Domain.Core.Roles;

public sealed class Permission : Entity
{
    public string Key { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Permission() { }

    public static Permission Create(string key, string description, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("La clave del permiso no puede estar vacía.", nameof(key));

        return new Permission
        {
            Key = key.Trim().ToLowerInvariant(),
            Description = description.Trim(),
            CreatedBy = createdBy
        };
    }
}
