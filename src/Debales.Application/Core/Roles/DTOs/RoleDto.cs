namespace Debales.Application.Core.Roles.DTOs;

public sealed record RoleDto(Guid Id, string Name, string Description, bool IsSystem);
