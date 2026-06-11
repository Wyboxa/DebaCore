namespace Debales.Application.Inventory.Commands.CloseInventoryCount;

public sealed record CloseInventoryCountCommand(
    Guid InventoryCountId,
    string UpdatedBy);
