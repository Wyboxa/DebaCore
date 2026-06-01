namespace Debales.Application.Inventory.Queries.GetStockBalance;

public sealed record GetStockBalanceQuery(Guid? ItemId, Guid? WarehouseId);
