namespace Debales.Application.Catalog.Commands.DeleteCustomerItemCode;

public sealed record DeleteCustomerItemCodeCommand(Guid CustomerId, Guid ItemId);
