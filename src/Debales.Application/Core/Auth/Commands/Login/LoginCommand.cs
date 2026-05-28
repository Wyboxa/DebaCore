namespace Debales.Application.Core.Auth.Commands.Login;

public sealed record LoginCommand(string UsernameOrEmail, string Password);
