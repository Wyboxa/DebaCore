namespace Debales.Application.Suppliers.Commands.UpdateSupplier;

public sealed record UpdateSupplierCommand(
    Guid SupplierId,
    string Name,
    string? TaxId,
    string? Phone,
    string? Email,
    string? Website,
    string? ContactName,
    string? Notes,
    string? AddressStreet,
    string? AddressCity,
    string? AddressPostalCode,
    string? AddressCountry,
    string UpdatedBy,
    string? AccountCode = null,
    Guid? PaymentTermId = null,
    Guid? PaymentMethodId = null);
