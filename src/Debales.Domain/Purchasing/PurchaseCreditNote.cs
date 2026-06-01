using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseCreditNote : AuditableEntity
{
    private readonly List<PurchaseCreditNoteLine> _lines = [];

    private PurchaseCreditNote() { }

    public string Number { get; private set; } = null!;
    public Guid SupplierId { get; private set; }
    public Guid OriginalInvoiceId { get; private set; }
    public DateOnly Date { get; private set; }
    public PurchaseCreditNoteStatus Status { get; private set; } = PurchaseCreditNoteStatus.Draft;
    public string? Reason { get; private set; }

    public Supplier? Supplier { get; private set; }
    public PurchaseInvoice? OriginalInvoice { get; private set; }

    public IReadOnlyList<PurchaseCreditNoteLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static PurchaseCreditNote Create(
        string number, Guid supplierId, Guid originalInvoiceId,
        DateOnly date, string? reason, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));
        if (originalInvoiceId == Guid.Empty) throw new ArgumentException("Original invoice is required.", nameof(originalInvoiceId));

        return new PurchaseCreditNote
        {
            Number = number.Trim().ToUpper(),
            SupplierId = supplierId,
            OriginalInvoiceId = originalInvoiceId,
            Date = date,
            Reason = reason?.Trim(),
            CreatedBy = createdBy
        };
    }

    public PurchaseCreditNoteLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != PurchaseCreditNoteStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft credit notes.");

        var line = PurchaseCreditNoteLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != PurchaseCreditNoteStatus.Draft)
            throw new InvalidOperationException("Only draft credit notes can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post a credit note with no lines.");

        Status = PurchaseCreditNoteStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == PurchaseCreditNoteStatus.Posted)
            throw new InvalidOperationException("Posted credit notes cannot be cancelled.");
        if (Status == PurchaseCreditNoteStatus.Cancelled)
            throw new InvalidOperationException("Credit note is already cancelled.");

        Status = PurchaseCreditNoteStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
