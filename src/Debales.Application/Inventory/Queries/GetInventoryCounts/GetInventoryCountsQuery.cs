using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetInventoryCounts;

public sealed record GetInventoryCountsQuery(
    Guid? WarehouseId,
    InventoryCountStatus? Status,
    int Page,
    int PageSize);
