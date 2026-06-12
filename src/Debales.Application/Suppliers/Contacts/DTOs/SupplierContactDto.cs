namespace Debales.Application.Suppliers.Contacts.DTOs;

public sealed record SupplierContactDto(
    Guid Id,
    Guid SupplierId,
    string FirstName,
    string LastName,
    string FullName,
    string? JobTitle,
    string? Email,
    string? Phone,
    bool IsActive,
    DateTime CreatedAt);
