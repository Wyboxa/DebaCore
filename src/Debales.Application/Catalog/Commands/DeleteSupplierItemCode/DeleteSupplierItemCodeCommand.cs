namespace Debales.Application.Catalog.Commands.DeleteSupplierItemCode;

public sealed record DeleteSupplierItemCodeCommand(Guid SupplierId, Guid ItemId);
