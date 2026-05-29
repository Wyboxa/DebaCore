using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesOrderSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    DateOnly? RequestedDeliveryDate,
    SalesOrderStatus Status,
    string StatusLabel,
    decimal Total);
