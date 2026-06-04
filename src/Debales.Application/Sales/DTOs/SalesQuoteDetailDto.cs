using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesQuoteDetailDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    DateOnly ValidUntil,
    SalesQuoteStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<SalesQuoteLineDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    Guid? ConvertedToOrderId,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
