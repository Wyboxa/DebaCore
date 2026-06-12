namespace Debales.Application.Suppliers.Contacts.Commands.UpdateSupplierContact;

public sealed record UpdateSupplierContactCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? JobTitle,
    string? Email,
    string? Phone,
    string UpdatedBy);
