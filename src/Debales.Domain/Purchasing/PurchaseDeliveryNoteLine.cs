using Debales.Domain.Common;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseDeliveryNoteLine : Entity
{
    private PurchaseDeliveryNoteLine() { }

    public Guid PurchaseDeliveryNoteId { get; private set; }
    public int SortOrder { get; private set; }

    public Guid? PurchaseOrderLineId { get; private set; }
    public Guid? PurchaseOrderId { get; private set; }

    // Snapshot
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public string? Description { get; private set; }

    public decimal Quantity { get; private set; }

    internal static PurchaseDeliveryNoteLine Create(
        Guid noteId, int sortOrder,
        Guid? purchaseOrderLineId, Guid? purchaseOrderId,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        return new PurchaseDeliveryNoteLine
        {
            PurchaseDeliveryNoteId = noteId,
            SortOrder = sortOrder,
            PurchaseOrderLineId = purchaseOrderLineId,
            PurchaseOrderId = purchaseOrderId,
            ItemId = itemId,
            ItemCode = itemCode,
            ItemName = itemName,
            Description = description?.Trim(),
            Quantity = quantity
        };
    }
}
