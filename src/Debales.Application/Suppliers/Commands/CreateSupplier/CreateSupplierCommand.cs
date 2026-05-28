namespace Debales.Application.Suppliers.Commands.CreateSupplier;

public sealed record CreateSupplierCommand(
    string Name,
    string? TaxId,
    string? Phone,
    string? Email,
    string? ContactName,
    string CreatedBy);
