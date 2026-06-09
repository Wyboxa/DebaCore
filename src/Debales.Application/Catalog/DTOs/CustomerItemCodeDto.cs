namespace Debales.Application.Catalog.DTOs;

public sealed record CustomerItemCodeDto(
    Guid Id,
    Guid CustomerId,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string CustomerCode,
    string? Description,
    DateTime CreatedAt);
