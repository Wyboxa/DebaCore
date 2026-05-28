namespace Debales.Application.Core.Auth.DTOs;

public sealed record LoginResultDto(
    Guid UserId,
    string Username,
    string Email,
    string Token,
    DateTime ExpiresAt
);
