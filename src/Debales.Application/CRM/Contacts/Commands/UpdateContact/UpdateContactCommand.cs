namespace Debales.Application.CRM.Contacts.Commands.UpdateContact;

public sealed record UpdateContactCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? JobTitle,
    string? Email,
    string? Phone,
    string UpdatedBy);
