namespace Debales.Application.Inventory.DTOs;

public sealed record InventoryCountSummaryDto(
    Guid Id,
    string Number,
    DateOnly Date,
    Guid WarehouseId,
    string WarehouseName,
    string Status,
    int LineCount,
    int CountedLines,
    DateTime CreatedAt,
    string? CreatedBy);

public sealed record InventoryCountDetailDto(
    Guid Id,
    string Number,
    DateOnly Date,
    Guid WarehouseId,
    string WarehouseName,
    string Status,
    string? Notes,
    IReadOnlyList<InventoryCountLineDto> Lines,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);

public sealed record InventoryCountLineDto(
    Guid Id,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    decimal SystemQuantity,
    decimal? CountedQuantity,
    decimal Difference,
    bool IsCounted);
