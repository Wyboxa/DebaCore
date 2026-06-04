namespace Debales.Application.Licensing.DTOs;

public sealed record LicenseModuleDto(
    Guid Id,
    string ModuleCode,
    DateTime GrantedAt);
