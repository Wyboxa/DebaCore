using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesQuoteSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    DateOnly ValidUntil,
    SalesQuoteStatus Status,
    string StatusLabel,
    decimal Total,
    Guid? ConvertedToOrderId);
