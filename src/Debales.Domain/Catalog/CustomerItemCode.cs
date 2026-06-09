using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class CustomerItemCode : Entity
{
    private CustomerItemCode() { }

    public Guid CustomerId { get; private set; }
    public Guid ItemId { get; private set; }
    public string CustomerCode { get; private set; } = null!;
    public string? Description { get; private set; }

    // Navigation (EF only)
    public Item Item { get; private set; } = null!;

    public static CustomerItemCode Create(
        Guid customerId, Guid itemId, string customerCode, string? description, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);
        return new CustomerItemCode
        {
            CustomerId = customerId,
            ItemId = itemId,
            CustomerCode = customerCode.Trim().ToUpper(),
            Description = description?.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Update(string customerCode, string? description, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerCode);
        CustomerCode = customerCode.Trim().ToUpper();
        Description = description?.Trim();
        SetUpdated(updatedBy);
    }
}
