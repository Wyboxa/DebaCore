using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class Item : AuditableEntity
{
    private Item() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsService { get; private set; }
    public bool IsActive { get; private set; } = true;
    public decimal SalePrice { get; private set; }
    public decimal PurchasePrice { get; private set; }

    public decimal? MinimumStock { get; private set; }

    public Guid? FamilyId { get; private set; }
    public Guid UnitOfMeasureId { get; private set; }
    public Guid? TaxTypeId { get; private set; }

    // Navigation properties (EF only — do not use outside Infrastructure)
    public ItemFamily? Family { get; private set; }
    public UnitOfMeasure UnitOfMeasure { get; private set; } = null!;
    public TaxType? TaxType { get; private set; }

    public static Item Create(
        string code, string name, string? description,
        bool isService, Guid unitOfMeasureId, Guid? familyId, Guid? taxTypeId,
        decimal salePrice, decimal purchasePrice, string createdBy,
        decimal? minimumStock = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (salePrice < 0) throw new ArgumentException("Sale price cannot be negative.", nameof(salePrice));
        if (purchasePrice < 0) throw new ArgumentException("Purchase price cannot be negative.", nameof(purchasePrice));
        if (minimumStock < 0) throw new ArgumentException("Minimum stock cannot be negative.", nameof(minimumStock));

        return new Item
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            Description = description?.Trim(),
            IsService = isService,
            UnitOfMeasureId = unitOfMeasureId,
            FamilyId = familyId,
            TaxTypeId = taxTypeId,
            SalePrice = salePrice,
            PurchasePrice = purchasePrice,
            MinimumStock = minimumStock,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string code, string name, string? description,
        bool isService, Guid unitOfMeasureId, Guid? familyId, Guid? taxTypeId,
        decimal salePrice, decimal purchasePrice, string updatedBy,
        decimal? minimumStock = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (salePrice < 0) throw new ArgumentException("Sale price cannot be negative.", nameof(salePrice));
        if (purchasePrice < 0) throw new ArgumentException("Purchase price cannot be negative.", nameof(purchasePrice));
        if (minimumStock < 0) throw new ArgumentException("Minimum stock cannot be negative.", nameof(minimumStock));

        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Description = description?.Trim();
        IsService = isService;
        UnitOfMeasureId = unitOfMeasureId;
        FamilyId = familyId;
        TaxTypeId = taxTypeId;
        SalePrice = salePrice;
        PurchasePrice = purchasePrice;
        MinimumStock = minimumStock;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
}
