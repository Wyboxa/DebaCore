namespace Debales.Application.Catalog.DTOs;

public sealed record ItemDetailDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsService,
    decimal SalePrice,
    decimal PurchasePrice,
    bool IsActive,
    Guid? FamilyId,
    string? FamilyName,
    Guid UnitOfMeasureId,
    string UnitOfMeasureName,
    Guid? TaxTypeId,
    string? TaxTypeName,
    decimal? TaxRate,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    decimal? MinimumStock = null);
