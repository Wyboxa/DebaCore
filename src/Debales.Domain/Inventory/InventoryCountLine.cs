using Debales.Domain.Catalog;
using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class InventoryCountLine : Entity
{
    private InventoryCountLine() { }

    public Guid InventoryCountId { get; private set; }
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public decimal SystemQuantity { get; private set; }
    public decimal? CountedQuantity { get; private set; }

    public decimal Difference => CountedQuantity.HasValue ? CountedQuantity.Value - SystemQuantity : 0m;
    public bool IsCounted => CountedQuantity.HasValue;

    public InventoryCount? InventoryCount { get; private set; }
    public Item? Item { get; private set; }

    internal static InventoryCountLine Create(
        Guid inventoryCountId, Guid itemId, string itemCode, string itemName,
        decimal systemQuantity, string createdBy)
    {
        return new InventoryCountLine
        {
            InventoryCountId = inventoryCountId,
            ItemId = itemId,
            ItemCode = itemCode,
            ItemName = itemName,
            SystemQuantity = systemQuantity,
            CountedQuantity = null,
            CreatedBy = createdBy
        };
    }

    internal void SetCounted(decimal countedQuantity, string updatedBy)
    {
        if (countedQuantity < 0)
            throw new ArgumentException("La cantidad contada no puede ser negativa.", nameof(countedQuantity));

        CountedQuantity = countedQuantity;
        SetUpdated(updatedBy);
    }
}
