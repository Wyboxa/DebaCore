namespace Debales.Application.Catalog.DTOs;

public sealed record ItemSummaryDto(
    Guid Id,
    string Code,
    string Name,
    string? FamilyName,
    string UnitOfMeasureName,
    bool IsService,
    decimal SalePrice,
    bool IsActive,
    decimal PurchasePrice = 0m);
