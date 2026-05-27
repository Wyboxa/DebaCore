namespace Debales.Application.Core.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string CreatedBy);
