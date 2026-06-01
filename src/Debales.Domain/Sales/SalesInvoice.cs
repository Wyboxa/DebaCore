using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class SalesInvoice : AuditableEntity
{
    private readonly List<SalesInvoiceLine> _lines = [];

    private SalesInvoice() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Guid? SalesDeliveryNoteId { get; private set; }
    public DateOnly Date { get; private set; }
    public DateOnly DueDate { get; private set; }
    public SalesInvoiceStatus Status { get; private set; } = SalesInvoiceStatus.Draft;
    public string? Notes { get; private set; }

    public Customer? Customer { get; private set; }
    public SalesDeliveryNote? SalesDeliveryNote { get; private set; }

    public IReadOnlyList<SalesInvoiceLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static SalesInvoice Create(
        string number, Guid customerId, Guid? salesDeliveryNoteId,
        DateOnly date, DateOnly dueDate, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));
        if (dueDate < date) throw new ArgumentException("Due date cannot be before invoice date.", nameof(dueDate));

        return new SalesInvoice
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            SalesDeliveryNoteId = salesDeliveryNoteId,
            Date = date,
            DueDate = dueDate,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public SalesInvoiceLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != SalesInvoiceStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft invoices.");

        var line = SalesInvoiceLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != SalesInvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post an invoice with no lines.");

        Status = SalesInvoiceStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == SalesInvoiceStatus.Posted)
            throw new InvalidOperationException("Posted invoices cannot be cancelled directly. Issue a credit note.");
        if (Status == SalesInvoiceStatus.Cancelled)
            throw new InvalidOperationException("Invoice is already cancelled.");

        Status = SalesInvoiceStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
