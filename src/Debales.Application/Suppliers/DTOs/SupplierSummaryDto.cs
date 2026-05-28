namespace Debales.Application.Suppliers.DTOs;

public sealed record SupplierSummaryDto(
    Guid Id,
    string Name,
    string? TaxId,
    string? Phone,
    string? Email,
    string? ContactName,
    bool IsActive,
    DateTime CreatedAt);
