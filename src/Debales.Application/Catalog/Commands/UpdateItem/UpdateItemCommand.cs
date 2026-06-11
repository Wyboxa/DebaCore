namespace Debales.Application.Catalog.Commands.UpdateItem;

public sealed record UpdateItemCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsService,
    Guid UnitOfMeasureId,
    Guid? FamilyId,
    Guid? TaxTypeId,
    decimal SalePrice,
    decimal PurchasePrice,
    string UpdatedBy,
    decimal? MinimumStock = null);
