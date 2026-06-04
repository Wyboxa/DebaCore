using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class SalesOrder : AuditableEntity
{
    private readonly List<SalesOrderLine> _lines = [];

    private SalesOrder() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public DateOnly Date { get; private set; }
    public DateOnly? RequestedDeliveryDate { get; private set; }
    public SalesOrderStatus Status { get; private set; } = SalesOrderStatus.Draft;
    public string? Notes { get; private set; }

    // Navigation properties (EF only — do not use outside Infrastructure)
    public Customer? Customer { get; private set; }

    public IReadOnlyList<SalesOrderLine> Lines => _lines.AsReadOnly();

    public decimal Subtotal => _lines.Sum(l => l.LineSubtotal);
    public decimal TaxAmount => _lines.Sum(l => l.LineTaxAmount);
    public decimal Total => _lines.Sum(l => l.LineTotal);

    public static SalesOrder Create(
        string number, Guid customerId, DateOnly date,
        DateOnly? requestedDeliveryDate, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));

        return new SalesOrder
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            Date = date,
            RequestedDeliveryDate = requestedDeliveryDate,
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }

    public SalesOrderLine AddLine(
        Guid itemId, string itemCode, string itemName, string? description,
        decimal quantity, decimal unitPrice, decimal taxRate)
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Lines can only be modified on draft orders.");

        var line = SalesOrderLine.Create(
            Id, _lines.Count + 1,
            itemId, itemCode, itemName, description,
            quantity, unitPrice, taxRate);

        _lines.Add(line);
        return line;
    }

    public void Confirm(string updatedBy)
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be confirmed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot confirm an order with no lines.");

        Status = SalesOrderStatus.Confirmed;
        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == SalesOrderStatus.Delivered || Status == SalesOrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order in status {Status}.");

        Status = SalesOrderStatus.Cancelled;
        SetUpdated(updatedBy);
    }

    public void UpdateDeliveryStatus(string updatedBy)
    {
        if (Status == SalesOrderStatus.Cancelled) return;

        bool allDelivered = _lines.All(l => l.PendingQuantity == 0);
        bool anyDelivered = _lines.Any(l => l.DeliveredQuantity > 0);

        Status = allDelivered
            ? SalesOrderStatus.Delivered
            : anyDelivered
                ? SalesOrderStatus.PartiallyDelivered
                : SalesOrderStatus.Confirmed;

        SetUpdated(updatedBy);
    }
}
