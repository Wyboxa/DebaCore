namespace Debales.Application.Suppliers.DTOs;

public sealed record SupplierDetailDto(
    Guid Id,
    string Name,
    string? TaxId,
    string? Phone,
    string? Email,
    string? Website,
    string? ContactName,
    string? Notes,
    bool IsActive,
    SupplierAddressDto? Address,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt);

public sealed record SupplierAddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country);
