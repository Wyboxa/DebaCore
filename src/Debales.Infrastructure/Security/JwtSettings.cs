namespace Debales.Infrastructure.Security;

public sealed class JwtSettings
{
    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = "Debales";
    public string Audience { get; init; } = "Debales";
    public int ExpirationHours { get; init; } = 8;
}
