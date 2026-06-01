using Debales.Domain.Common;
using Debales.Domain.Suppliers;

namespace Debales.Domain.Purchasing;

public sealed class SupplierPayment : AuditableEntity
{
    private SupplierPayment() { }

    public string Number { get; private set; } = null!;
    public Guid SupplierId { get; private set; }
    public Guid? PayableId { get; private set; }
    public DateOnly Date { get; private set; }
    public decimal Amount { get; private set; }
    public string? Reference { get; private set; }
    public string? Notes { get; private set; }

    public Supplier? Supplier { get; private set; }
    public Payable? Payable { get; private set; }

    public static SupplierPayment Create(
        string number, Guid supplierId, Guid? payableId,
        DateOnly date, decimal amount, string? reference, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (supplierId == Guid.Empty) throw new ArgumentException("Supplier is required.", nameof(supplierId));
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));

        return new SupplierPayment
        {
            Number = number.Trim().ToUpper(),
            SupplierId = supplierId,
            PayableId = payableId,
            Date = date,
            Amount = amount,
            Reference = reference?.Trim(),
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }
}
