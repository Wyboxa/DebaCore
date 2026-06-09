namespace Debales.Application.Catalog.Commands.UpsertCustomerItemCode;

public sealed record UpsertCustomerItemCodeCommand(
    Guid CustomerId,
    Guid ItemId,
    string CustomerCode,
    string? Description,
    string UpdatedBy);
