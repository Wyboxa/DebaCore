using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesInvoiceSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    DateOnly DueDate,
    SalesInvoiceStatus Status,
    string StatusLabel,
    decimal Total);
