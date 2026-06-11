namespace Debales.Application.Inventory.DTOs;

public sealed record StockBalanceDto(
    Guid ItemId,
    string ItemCode,
    string ItemName,
    Guid WarehouseId,
    string WarehouseName,
    decimal Quantity,
    decimal? MinimumStock = null);
