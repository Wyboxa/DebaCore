using Debales.Domain.Common;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseInvoiceLine : Entity
{
    private PurchaseInvoiceLine() { }

    public Guid PurchaseInvoiceId { get; private set; }
    public int SortOrder { get; private set; }

    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;
    public string? Description { get; private set; }

    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TaxRate { get; private set; }

    public decimal LineSubtotal => Quantity * UnitPrice;
    public decimal LineTaxAmount => LineSubtotal * TaxRate / 100m;
    public decimal LineTotal => LineSubtotal + LineTaxAmount;

    internal static PurchaseInvoiceLine Create(
        Guid purchaseInvoiceId, int sortOrder,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
        if (taxRate < 0 || taxRate > 100) throw new ArgumentException("Tax rate must be between 0 and 100.", nameof(taxRate));

        return new PurchaseInvoiceLine
        {
            PurchaseInvoiceId = purchaseInvoiceId,
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
}
