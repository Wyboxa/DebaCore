using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.DTOs;

public sealed record StockMovementSummaryDto(
    Guid Id,
    string Number,
    StockMovementType Type,
    string TypeLabel,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    Guid WarehouseId,
    string WarehouseName,
    string? LocationCode,
    DateOnly Date,
    decimal Quantity,
    string? Reference);

public sealed record StockMovementDetailDto(
    Guid Id,
    string Number,
    StockMovementType Type,
    string TypeLabel,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    Guid WarehouseId,
    string WarehouseName,
    Guid? LocationId,
    string? LocationCode,
    DateOnly Date,
    decimal Quantity,
    string? Reference,
    string? Notes,
    DateTime CreatedAt,
    string? CreatedBy);
