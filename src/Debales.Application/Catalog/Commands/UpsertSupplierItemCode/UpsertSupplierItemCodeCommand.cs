namespace Debales.Application.Catalog.Commands.UpsertSupplierItemCode;

public sealed record UpsertSupplierItemCodeCommand(
    Guid SupplierId,
    Guid ItemId,
    string SupplierCode,
    string? Description,
    string UpdatedBy);
