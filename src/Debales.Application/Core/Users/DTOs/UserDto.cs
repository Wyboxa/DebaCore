namespace Debales.Application.Core.Users.DTOs;

public sealed record UserDto(
    Guid Id,
    string Username,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    string? CreatedBy);
