using Debales.Domain.Common;
using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Sales;

public sealed class CustomerPayment : AuditableEntity
{
    private CustomerPayment() { }

    public string Number { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Guid? ReceivableId { get; private set; }
    public DateOnly Date { get; private set; }
    public decimal Amount { get; private set; }
    public string? Reference { get; private set; }
    public string? Notes { get; private set; }

    public Customer? Customer { get; private set; }
    public Receivable? Receivable { get; private set; }

    public static CustomerPayment Create(
        string number, Guid customerId, Guid? receivableId,
        DateOnly date, decimal amount, string? reference, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.", nameof(customerId));
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));

        return new CustomerPayment
        {
            Number = number.Trim().ToUpper(),
            CustomerId = customerId,
            ReceivableId = receivableId,
            Date = date,
            Amount = amount,
            Reference = reference?.Trim(),
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }
}
