namespace Debales.Application.Inventory.Commands.SetCountedQuantity;

public sealed record SetCountedQuantityCommand(
    Guid InventoryCountId,
    Guid ItemId,
    decimal CountedQuantity,
    string UpdatedBy);
