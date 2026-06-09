namespace Debales.Application.Catalog.DTOs;

public sealed record PriceListSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    int ItemCount,
    DateTime CreatedAt,
    string? CreatedBy);
