namespace Debales.Application.CRM.Contacts.Commands.AddContact;

public sealed record AddContactCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string? JobTitle,
    string? Email,
    string? Phone,
    string CreatedBy);
