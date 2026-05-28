namespace Debales.Application.Catalog.Commands.CreateItem;

public sealed record CreateItemCommand(
    string Code,
    string Name,
    string? Description,
    bool IsService,
    Guid UnitOfMeasureId,
    Guid? FamilyId,
    Guid? TaxTypeId,
    decimal SalePrice,
    decimal PurchasePrice,
    string CreatedBy);
