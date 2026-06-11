namespace Debales.Application.Inventory.Commands.CreateInventoryCount;

public sealed record CreateInventoryCountCommand(
    Guid WarehouseId,
    DateOnly Date,
    string? Notes,
    string CreatedBy);
