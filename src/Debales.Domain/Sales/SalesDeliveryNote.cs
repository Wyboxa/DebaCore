using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class SalesDeliveryNote : AuditableEntity
{
    private readonly List<SalesDeliveryNoteLine> _lines = [];

    private SalesDeliveryNote() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Guid? SalesOrderId { get; private set; }
    public DateOnly Date { get; private set; }
    public SalesDeliveryNoteStatus Status { get; private set; } = SalesDeliveryNoteStatus.Draft;
    public string? Notes { get; private set; }

    // Navigation properties (EF only — do not use outside Infrastructure)
    public Customer? Customer { get; private set; }
    public SalesOrder? SalesOrder { get; private set; }

    public IReadOnlyList<SalesDeliveryNoteLine> Lines => _lines.AsReadOnly();

    public static SalesDeliveryNote Create(
        string number, Guid customerId, Guid? salesOrderId,
        DateOnly date, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));

        return new SalesDeliveryNote
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            SalesOrderId = salesOrderId,
            Date = date,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public SalesDeliveryNoteLine AddLine(
        Guid? salesOrderLineId, Guid? salesOrderId,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity)
    {
        if (Status != SalesDeliveryNoteStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft delivery notes.");

        var line = SalesDeliveryNoteLine.Create(
            Id, _lines.Count + 1,
            salesOrderLineId, salesOrderId,
            itemId, itemCode, itemName, description,
            quantity);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != SalesDeliveryNoteStatus.Draft)
            throw new InvalidOperationException("Only draft delivery notes can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post a delivery note with no lines.");

        Status = SalesDeliveryNoteStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == SalesDeliveryNoteStatus.Cancelled)
            throw new InvalidOperationException("Delivery note is already cancelled.");

        Status = SalesDeliveryNoteStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
