using Debales.Domain.Common;

namespace Debales.Domain.Sales;

public sealed class SalesDeliveryNoteLine : Entity
{
    private SalesDeliveryNoteLine() { }

    public Guid SalesDeliveryNoteId { get; private set; }
    public int SortOrder { get; private set; }

    public Guid? SalesOrderLineId { get; private set; }
    public Guid? SalesOrderId { get; private set; }

    // Snapshot
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public string? Description { get; private set; }

    public decimal Quantity { get; private set; }

    internal static SalesDeliveryNoteLine Create(
        Guid noteId, int sortOrder,
        Guid? salesOrderLineId, Guid? salesOrderId,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        return new SalesDeliveryNoteLine
        {
            SalesDeliveryNoteId = noteId,
            SortOrder = sortOrder,
            SalesOrderLineId = salesOrderLineId,
            SalesOrderId = salesOrderId,
            ItemId = itemId,
            ItemCode = itemCode,
            ItemName = itemName,
            Description = description?.Trim(),
            Quantity = quantity
        };
    }
}
