using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class Payable : AuditableEntity
{
    private Payable() { }

    public string Number { get; private set; } = null!;
    public Guid PurchaseInvoiceId { get; private set; }
    public Guid SupplierId { get; private set; }
    public DateOnly DueDate { get; private set; }
    public decimal OriginalAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public decimal PendingAmount => OriginalAmount - PaidAmount;
    public PayableStatus Status { get; private set; } = PayableStatus.Pending;

    public PurchaseInvoice? PurchaseInvoice { get; private set; }
    public Supplier? Supplier { get; private set; }

    public static Payable Create(
        string number, Guid purchaseInvoiceId, Guid supplierId,
        DateOnly dueDate, decimal amount, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (purchaseInvoiceId == Guid.Empty) throw new ArgumentException("Purchase invoice is required.", nameof(purchaseInvoiceId));
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));
        if (amount == 0) throw new ArgumentException("Amount cannot be zero.", nameof(amount));

        return new Payable
        {
            Number = number.Trim().ToUpper(),
            PurchaseInvoiceId = purchaseInvoiceId,
            SupplierId = supplierId,
            DueDate = dueDate,
            OriginalAmount = amount,
            CreatedBy = createdBy
        };
    }

    public void ApplyPayment(decimal amount, string updatedBy)
    {
        if (Status == PayableStatus.Settled || Status == PayableStatus.Cancelled)
            throw new InvalidOperationException($"Cannot apply payment to a {Status} payable.");
        if (amount <= 0) throw new ArgumentException("Payment amount must be positive.", nameof(amount));

        var newPaid = PaidAmount + amount;
        if (OriginalAmount > 0 && newPaid > OriginalAmount)
            throw new InvalidOperationException("Payment exceeds the pending amount.");

        PaidAmount = newPaid;
        Status = Math.Abs(PaidAmount) >= Math.Abs(OriginalAmount)
            ? PayableStatus.Settled
            : PayableStatus.Partial;

        SetUpdated(updatedBy);
    }

    public void Cancel(string updatedBy)
    {
        if (Status == PayableStatus.Settled)
            throw new InvalidOperationException("Cannot cancel a settled payable.");

        Status = PayableStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
