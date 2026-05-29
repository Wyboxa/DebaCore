using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class PurchaseDeliveryNote : AuditableEntity
{
    private readonly List<PurchaseDeliveryNoteLine> _lines = [];

    private PurchaseDeliveryNote() { }

    public string Number { get; private set; } = null!;
    public Guid SupplierId { get; private set; }
    public Guid? PurchaseOrderId { get; private set; }
    public DateOnly Date { get; private set; }
    public PurchaseDeliveryNoteStatus Status { get; private set; } = PurchaseDeliveryNoteStatus.Draft;
    public string? Notes { get; private set; }

    // Navigation properties (EF only — do not use outside Infrastructure)
    public Supplier? Supplier { get; private set; }
    public PurchaseOrder? PurchaseOrder { get; private set; }

    public IReadOnlyList<PurchaseDeliveryNoteLine> Lines => _lines.AsReadOnly();

    public static PurchaseDeliveryNote Create(
        string number, Guid supplierId, Guid? purchaseOrderId,
        DateOnly date, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));

        return new PurchaseDeliveryNote
        {
            Number = number.Trim().ToUpper(),
            SupplierId = supplierId,
            PurchaseOrderId = purchaseOrderId,
            Date = date,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public PurchaseDeliveryNoteLine AddLine(
        Guid? purchaseOrderLineId, Guid? purchaseOrderId,
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity)
    {
        if (Status != PurchaseDeliveryNoteStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft delivery notes.");

        var line = PurchaseDeliveryNoteLine.Create(
            Id, _lines.Count + 1,
            purchaseOrderLineId, purchaseOrderId,
            itemId, itemCode, itemName, description,
            quantity);

        _lines.Add(line);
        return line;
    }

    public void Post(string updatedBy)
    {
        if (Status != PurchaseDeliveryNoteStatus.Draft)
            throw new InvalidOperationException("Only draft delivery notes can be posted.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot post a delivery note with no lines.");

        Status = PurchaseDeliveryNoteStatus.Posted;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == PurchaseDeliveryNoteStatus.Cancelled)
            throw new InvalidOperationException("Delivery note is already cancelled.");

        Status = PurchaseDeliveryNoteStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
