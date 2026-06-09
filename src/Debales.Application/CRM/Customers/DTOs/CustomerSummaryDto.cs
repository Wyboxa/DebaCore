namespace Debales.Application.CRM.Customers.DTOs;

public sealed record CustomerSummaryDto(
    Guid Id,
    string Name,
    string? Sector,
    string? TaxId,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTime CreatedAt,
    Guid? PriceListId = null,
    Guid? PaymentTermId = null);
