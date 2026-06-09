using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class SupplierItemCode : Entity
{
    private SupplierItemCode() { }

    public Guid SupplierId { get; private set; }
    public Guid ItemId { get; private set; }
    public string SupplierCode { get; private set; } = null!;
    public string? Description { get; private set; }

    // Navigation (EF only)
    public Item Item { get; private set; } = null!;

    public static SupplierItemCode Create(
        Guid supplierId, Guid itemId, string supplierCode, string? description, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierCode);
        return new SupplierItemCode
        {
            SupplierId = supplierId,
            ItemId = itemId,
            SupplierCode = supplierCode.Trim().ToUpper(),
            Description = description?.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Update(string supplierCode, string? description, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(supplierCode);
        SupplierCode = supplierCode.Trim().ToUpper();
        Description = description?.Trim();
        SetUpdated(updatedBy);
    }
}
