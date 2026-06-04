namespace Debales.Application.Core.Users.Commands.ReactivateUser;

public sealed record ReactivateUserCommand(Guid UserId, string UpdatedBy);
