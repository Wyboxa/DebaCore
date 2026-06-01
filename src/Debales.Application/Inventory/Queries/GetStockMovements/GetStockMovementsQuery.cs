using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetStockMovements;

public sealed record GetStockMovementsQuery(
    string? Search, Guid? ItemId, Guid? WarehouseId, StockMovementType? Type, int Page, int PageSize);
