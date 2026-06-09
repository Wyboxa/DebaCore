namespace Debales.Application.Catalog.DTOs;

public sealed record ItemPriceDto(
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string? UnitOfMeasure,
    decimal Price);
