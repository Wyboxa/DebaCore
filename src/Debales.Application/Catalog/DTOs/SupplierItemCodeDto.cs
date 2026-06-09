namespace Debales.Application.Catalog.DTOs;

public sealed record SupplierItemCodeDto(
    Guid Id,
    Guid SupplierId,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string SupplierCode,
    string? Description,
    DateTime CreatedAt);
