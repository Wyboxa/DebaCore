namespace Debales.Application.CRM.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string Name,
    string? Sector,
    string? TaxId,
    string? Phone,
    string? Website,
    string UpdatedBy);
