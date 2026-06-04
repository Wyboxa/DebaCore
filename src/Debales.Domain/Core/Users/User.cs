using Debales.Domain.Common;
using Debales.Domain.Core.Roles;

namespace Debales.Domain.Core.Users;

public sealed class User : AuditableEntity
{
    private readonly List<UserRole> _userRoles = [];

    public string Username { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();

    private User() { }

    public static User Create(string username, Email email, string passwordHash, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(username));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de contraseña no puede estar vacío.", nameof(passwordHash));

        return new User
        {
            Username = username.Trim(),
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void AssignRole(Role role)
    {
        if (_userRoles.Any(ur => ur.RoleId == role.Id))
            return;

        _userRoles.Add(new UserRole(Id, role.Id));
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    public void Reactivate(string updatedBy)
    {
        IsActive = true;
        SetUpdated(updatedBy);
    }

    public void UpdateEmail(Email newEmail, string updatedBy)
    {
        Email = newEmail;
        SetUpdated(updatedBy);
    }

    public void UpdatePasswordHash(string newPasswordHash, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("El hash de contraseña no puede estar vacío.", nameof(newPasswordHash));
        PasswordHash = newPasswordHash;
        SetUpdated(updatedBy);
    }

    public void RemoveRole(Guid roleId)
    {
        var ur = _userRoles.FirstOrDefault(r => r.RoleId == roleId);
        if (ur is not null) _userRoles.Remove(ur);
    }
}
