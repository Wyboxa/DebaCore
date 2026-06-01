using Debales.Domain.Catalog;
using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class StockBalance : Entity
{
    private StockBalance() { }

    public Guid ItemId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public decimal Quantity { get; private set; }

    // Navigation (EF only)
    public Item? Item { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public static StockBalance Create(Guid itemId, Guid warehouseId)
    {
        if (itemId == Guid.Empty) throw new ArgumentException("Item is required.", nameof(itemId));
        if (warehouseId == Guid.Empty) throw new ArgumentException("Warehouse is required.", nameof(warehouseId));

        return new StockBalance { ItemId = itemId, WarehouseId = warehouseId };
    }

    public void Apply(decimal signedQuantity)
    {
        Quantity += signedQuantity;
    }
}
