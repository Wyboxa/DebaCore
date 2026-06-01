namespace Debales.Application.Inventory.DTOs;

public sealed record WarehouseSummaryDto(
    Guid Id, string Code, string Name, string? Description, bool IsActive);

public sealed record WarehouseDetailDto(
    Guid Id, string Code, string Name, string? Description, bool IsActive,
    IReadOnlyList<WarehouseLocationDto> Locations,
    DateTime CreatedAt, string? CreatedBy);

public sealed record WarehouseLocationDto(
    Guid Id, Guid WarehouseId, string Code, string? Description, bool IsActive);
