namespace Debales.Application.CRM.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string? Sector,
    string? TaxId,
    string? Phone,
    string? Email,
    string CreatedBy);
