namespace Debales.Application.Inventory.Commands.AddInventoryCountLine;

public sealed record AddInventoryCountLineCommand(
    Guid InventoryCountId,
    Guid ItemId,
    string CreatedBy);
