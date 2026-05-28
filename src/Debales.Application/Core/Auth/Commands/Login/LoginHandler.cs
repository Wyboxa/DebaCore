using Debales.Application.Common;
using Debales.Application.Core.Auth.DTOs;
using Debales.Application.Core.Users;

namespace Debales.Application.Core.Auth.Commands.Login;

public sealed class LoginHandler
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository users, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResultDto> Handle(LoginCommand command, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(command.UsernameOrEmail, ct)
                   ?? await _users.GetByEmailAsync(command.UsernameOrEmail, ct);

        if (user is null || !user.IsActive || !_passwordHasher.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var token = _tokenService.GenerateToken(user.Id, user.Username, []);
        return new LoginResultDto(user.Id, user.Username, user.Email.Value, token, _tokenService.GetExpirationTime());
    }
}
