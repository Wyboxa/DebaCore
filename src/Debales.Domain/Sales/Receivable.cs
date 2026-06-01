using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class Receivable : AuditableEntity
{
    private Receivable() { }

    public string Number { get; private set; } = null!;
    public Guid SalesInvoiceId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateOnly DueDate { get; private set; }
    public decimal OriginalAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public decimal PendingAmount => OriginalAmount - PaidAmount;
    public ReceivableStatus Status { get; private set; } = ReceivableStatus.Pending;

    public SalesInvoice? SalesInvoice { get; private set; }
    public Customer? Customer { get; private set; }

    public static Receivable Create(
        string number, Guid salesInvoiceId, Guid customerId,
        DateOnly dueDate, decimal amount, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (salesInvoiceId == Guid.Empty) throw new ArgumentException("Sales invoice is required.", nameof(salesInvoiceId));
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));
        if (amount == 0) throw new ArgumentException("Amount cannot be zero.", nameof(amount));

        return new Receivable
        {
            Number = number.Trim().ToUpper(),
            SalesInvoiceId = salesInvoiceId,
            CustomerId = customerId,
            DueDate = dueDate,
            OriginalAmount = amount,
            CreatedBy = createdBy
        };
    }

    public void ApplyPayment(decimal amount, string updatedBy)
    {
        if (Status == ReceivableStatus.Settled || Status == ReceivableStatus.Cancelled)
            throw new InvalidOperationException($"Cannot apply payment to a {Status} receivable.");
        if (amount <= 0) throw new ArgumentException("Payment amount must be positive.", nameof(amount));

        // For credit notes the OriginalAmount can be negative; allow settling in that direction
        var newPaid = PaidAmount + amount;
        if (OriginalAmount > 0 && newPaid > OriginalAmount)
            throw new InvalidOperationException("Payment exceeds the pending amount.");

        PaidAmount = newPaid;
        Status = Math.Abs(PaidAmount) >= Math.Abs(OriginalAmount)
            ? ReceivableStatus.Settled
            : ReceivableStatus.Partial;

        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == ReceivableStatus.Settled)
            throw new InvalidOperationException("Cannot cancel a settled receivable.");

        Status = ReceivableStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
