namespace Debales.Application.Catalog.DTOs;

public sealed record PriceListDetailDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    IReadOnlyList<ItemPriceDto> Items,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
