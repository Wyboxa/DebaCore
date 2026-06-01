namespace Debales.Application.Inventory.Commands.CreateWarehouse;

public sealed record CreateWarehouseCommand(string Code, string Name, string? Description, string CreatedBy);
