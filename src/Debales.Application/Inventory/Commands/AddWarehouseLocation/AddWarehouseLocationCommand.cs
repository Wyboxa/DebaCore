namespace Debales.Application.Inventory.Commands.AddWarehouseLocation;

public sealed record AddWarehouseLocationCommand(Guid WarehouseId, string Code, string? Description, string CreatedBy);
