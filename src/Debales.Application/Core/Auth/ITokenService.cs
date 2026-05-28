namespace Debales.Application.Core.Auth;

public interface ITokenService
{
    string GenerateToken(Guid userId, string username, IEnumerable<string> roles);
    DateTime GetExpirationTime();
}
