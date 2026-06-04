namespace Debales.Application.Core.Users.DTOs;

public sealed record UserSummaryDto(
    Guid Id,
    string Username,
    string Email,
    bool IsActive,
    IReadOnlyList<string> Roles,
    DateTime CreatedAt);
