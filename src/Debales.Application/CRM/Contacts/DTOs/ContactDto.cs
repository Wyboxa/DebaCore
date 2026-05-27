namespace Debales.Application.CRM.Contacts.DTOs;

public sealed record ContactDto(
    Guid Id,
    Guid CustomerId,
    string FirstName,
    string LastName,
    string FullName,
    string? JobTitle,
    string? Email,
    string? Phone,
    bool IsActive,
    DateTime CreatedAt);
