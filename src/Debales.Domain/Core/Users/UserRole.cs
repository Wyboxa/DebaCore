namespace Debales.Domain.Core.Users;

public sealed class UserRole
{
    public Guid UserId { get; }
    public Guid RoleId { get; }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    private UserRole() { }
}
