using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.CreateStockMovement;

public sealed record CreateStockMovementCommand(
    StockMovementType Type,
    Guid ItemId,
    Guid WarehouseId,
    Guid? LocationId,
    DateOnly Date,
    decimal Quantity,
    string? Reference,
    string? Notes,
    string CreatedBy);
