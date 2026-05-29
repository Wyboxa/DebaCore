using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseOrder : AuditableEntity
{
    private readonly List<PurchaseOrderLine> _lines = [];

    private PurchaseOrder() { }

    public string Number { get; private set; } = null!;
    public Guid SupplierId { get; private set; }
    public DateOnly Date { get; private set; }
    public DateOnly? ExpectedReceiptDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
    public string? Notes { get; private set; }

    // Navigation properties (EF only — do not use outside Infrastructure)
    public Supplier? Supplier { get; private set; }

    public IReadOnlyList<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static PurchaseOrder Create(
        string number, Guid supplierId, DateOnly date,
        DateOnly? expectedReceiptDate, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));

        return new PurchaseOrder
        {
            Number = number.Trim().ToUpper(),
            SupplierId = supplierId,
            Date = date,
            ExpectedReceiptDate = expectedReceiptDate,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public PurchaseOrderLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft orders.");

        var line = PurchaseOrderLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Confirm(string updatedBy)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be confirmed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot confirm an order with no lines.");

        Status = PurchaseOrderStatus.Confirmed;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == PurchaseOrderStatus.Received || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order in status {Status}.");

        Status = PurchaseOrderStatus.Cancelled;
        SetUpdated(updatedBy);
    }

    internal void UpdateReceiptStatus(string updatedBy)
    {
        if (Status == PurchaseOrderStatus.Cancelled) return;

        bool allReceived = _lines.All(l => l.PendingQuantity == 0);
        bool anyReceived = _lines.Any(l => l.ReceivedQuantity > 0);

        Status = allReceived
            ? PurchaseOrderStatus.Received
            : anyReceived
                ? PurchaseOrderStatus.PartiallyReceived
                : PurchaseOrderStatus.Confirmed;

        SetUpdated(updatedBy);
    }
}
