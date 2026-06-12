namespace Debales.Application.Suppliers.Contacts.Commands.AddSupplierContact;

public sealed record AddSupplierContactCommand(
    Guid SupplierId,
    string FirstName,
    string LastName,
    string? JobTitle,
    string? Email,
    string? Phone,
    string CreatedBy);
