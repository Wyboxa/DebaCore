namespace Debales.Application.Core.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(Guid UserId, string NewPassword, string UpdatedBy);
