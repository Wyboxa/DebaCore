namespace Debales.Application.CRM.Customers.DTOs;

public sealed record CustomerDetailDto(
    Guid Id,
    string Name,
    string? Sector,
    string? TaxId,
    string? Phone,
    string? Email,
    string? Website,
    bool IsActive,
    AddressDto? Address,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt);

public sealed record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country);
