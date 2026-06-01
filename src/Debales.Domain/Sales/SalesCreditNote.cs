using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class SalesCreditNote : AuditableEntity
{
    private readonly List<SalesCreditNoteLine> _lines = [];

    private SalesCreditNote() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Guid OriginalInvoiceId { get; private set; }
    public DateOnly Date { get; private set; }
    public SalesCreditNoteStatus Status { get; private set; } = SalesCreditNoteStatus.Draft;
    public string? Reason { get; private set; }

    public Customer? Customer { get; private set; }
    public SalesInvoice? OriginalInvoice { get; private set; }

    public IReadOnlyList<SalesCreditNoteLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static SalesCreditNote Create(
        string number, Guid customerId, Guid originalInvoiceId,
        DateOnly date, string? reason, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));
        if (originalInvoiceId == Guid.Empty) throw new ArgumentException("Original invoice is required.", nameof(originalInvoiceId));

        return new SalesCreditNote
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            OriginalInvoiceId = originalInvoiceId,
            Date = date,
            Reason = reason?.Trim(),
            CreatedBy = createdBy
        };
    }

    public SalesCreditNoteLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != SalesCreditNoteStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft credit notes.");

        var line = SalesCreditNoteLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != SalesCreditNoteStatus.Draft)
            throw new InvalidOperationException("Only draft credit notes can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post a credit note with no lines.");

        Status = SalesCreditNoteStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == SalesCreditNoteStatus.Posted)
            throw new InvalidOperationException("Posted credit notes cannot be cancelled.");
        if (Status == SalesCreditNoteStatus.Cancelled)
            throw new InvalidOperationException("Credit note is already cancelled.");

        Status = SalesCreditNoteStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
