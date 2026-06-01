using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseInvoice : AuditableEntity
{
    private readonly List<PurchaseInvoiceLine> _lines = [];

    private PurchaseInvoice() { }

    public string Number { get; private set; } = null!;
    public string? SupplierInvoiceNumber { get; private set; }
    public Guid SupplierId { get; private set; }
    public Guid? PurchaseDeliveryNoteId { get; private set; }
    public DateOnly Date { get; private set; }
    public DateOnly DueDate { get; private set; }
    public PurchaseInvoiceStatus Status { get; private set; } = PurchaseInvoiceStatus.Draft;
    public string? Notes { get; private set; }

    public Supplier? Supplier { get; private set; }
    public PurchaseDeliveryNote? PurchaseDeliveryNote { get; private set; }

    public IReadOnlyList<PurchaseInvoiceLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static PurchaseInvoice Create(
        string number, string? supplierInvoiceNumber, Guid supplierId,
        Guid? purchaseDeliveryNoteId, DateOnly date, DateOnly dueDate,
        string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));
        if (dueDate < date) throw new ArgumentException("Due date cannot be before invoice date.", nameof(dueDate));

        return new PurchaseInvoice
        {
            Number = number.Trim().ToUpper(),
            SupplierInvoiceNumber = supplierInvoiceNumber?.Trim(),
            SupplierId = supplierId,
            PurchaseDeliveryNoteId = purchaseDeliveryNoteId,
            Date = date,
            DueDate = dueDate,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public PurchaseInvoiceLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != PurchaseInvoiceStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft invoices.");

        var line = PurchaseInvoiceLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != PurchaseInvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post an invoice with no lines.");

        Status = PurchaseInvoiceStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == PurchaseInvoiceStatus.Posted)
            throw new InvalidOperationException("Posted invoices cannot be cancelled directly. Issue a credit note.");
        if (Status == PurchaseInvoiceStatus.Cancelled)
            throw new InvalidOperationException("Invoice is already cancelled.");

        Status = PurchaseInvoiceStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
