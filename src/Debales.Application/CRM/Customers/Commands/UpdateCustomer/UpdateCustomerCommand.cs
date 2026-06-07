namespace Debales.Application.CRM.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string Name,
    string? Sector,
    string? TaxId,
    string? Phone,
    string? Email,
    string? Website,
    string? AddressStreet,
    string? AddressCity,
    string? AddressPostalCode,
    string? AddressCountry,
    string UpdatedBy,
    string? AccountCode = null);
