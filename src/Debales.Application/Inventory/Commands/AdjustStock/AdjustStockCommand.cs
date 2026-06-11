namespace Debales.Application.Inventory.Commands.AdjustStock;

public sealed record AdjustStockCommand(
    Guid ItemId,
    Guid WarehouseId,
    decimal TargetQuantity,
    string? Reason,
    string CreatedBy);
