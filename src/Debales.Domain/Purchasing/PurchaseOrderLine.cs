using Debales.Domain.Common;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseOrderLine : Entity
{
    private PurchaseOrderLine() { }

    public Guid PurchaseOrderId { get; private set; }
    public int SortOrder { get; private set; }

    // Snapshot of item data at order time
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public string? Description { get; private set; }

    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TaxRate { get; private set; }

    public decimal ReceivedQuantity { get; private set; }

    public decimal LineSubtotal => Quantity * UnitPrice;
    public decimal LineTaxAmount => LineSubtotal * TaxRate / 100m;
    public decimal LineTotal => LineSubtotal + LineTaxAmount;

    public decimal PendingQuantity => Quantity - ReceivedQuantity;

    internal static PurchaseOrderLine Create(
        Guid purchaseOrderId, int sortOrder,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (taxRate < 0 || taxRate > 100) throw new ArgumentException("Tax rate must be between 0 and 100.", nameof(taxRate));

        return new PurchaseOrderLine
        {
            PurchaseOrderId = purchaseOrderId,
            SortOrder = sortOrder,
            ItemId = itemId,
            ItemCode = itemCode,
            ItemName = itemName,
            Description = description?.Trim(),
            Quantity = quantity,
            UnitPrice = unitPrice,
            TaxRate = taxRate
        };
    }

    internal void RecordReceipt(decimal receivedQty)
    {
        if (receivedQty <= 0) throw new ArgumentException("Received quantity must be positive.", nameof(receivedQty));
        if (ReceivedQuantity + receivedQty > Quantity)
            throw new InvalidOperationException("Cannot receive more than the ordered quantity.");

        ReceivedQuantity += receivedQty;
    }
}
