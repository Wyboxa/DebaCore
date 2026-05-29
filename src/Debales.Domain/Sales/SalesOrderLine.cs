using Debales.Domain.Common;

namespace Debales.Domain.Sales;

public sealed class SalesOrderLine : Entity
{
    private SalesOrderLine() { }

    public Guid SalesOrderId { get; private set; }
    public int SortOrder { get; private set; }

    // Snapshot of item data at order time
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public string? Description { get; private set; }

    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TaxRate { get; private set; }

    public decimal DeliveredQuantity { get; private set; }

    public decimal LineSubtotal => Quantity * UnitPrice;
    public decimal LineTaxAmount => LineSubtotal * TaxRate / 100m;
    public decimal LineTotal => LineSubtotal + LineTaxAmount;

    public decimal PendingQuantity => Quantity - DeliveredQuantity;

    internal static SalesOrderLine Create(
        Guid salesOrderId, int sortOrder,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (taxRate < 0 || taxRate > 100) throw new ArgumentException("Tax rate must be between 0 and 100.", nameof(taxRate));

        return new SalesOrderLine
        {
            SalesOrderId = salesOrderId,
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

    internal void RecordDelivery(decimal deliveredQty)
    {
        if (deliveredQty <= 0) throw new ArgumentException("Delivered quantity must be positive.", nameof(deliveredQty));
        if (DeliveredQuantity + deliveredQty > Quantity)
            throw new InvalidOperationException("Cannot deliver more than the ordered quantity.");

        DeliveredQuantity += deliveredQty;
    }
}
