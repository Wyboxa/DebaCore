namespace Debales.Application.Core.Users.Commands.AssignRole;

public sealed record AssignRoleCommand(Guid UserId, Guid RoleId, string UpdatedBy);
