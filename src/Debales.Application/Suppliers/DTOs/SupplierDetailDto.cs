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
    DateTime? UpdatedAt,
    string? AccountCode = null,
    Guid? PaymentTermId = null,
    string? PaymentTermName = null,
    Guid? PaymentMethodId = null,
    string? PaymentMethodName = null);

public sealed record SupplierAddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country);
