namespace Debales.Application.Core.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId, string UpdatedBy);
