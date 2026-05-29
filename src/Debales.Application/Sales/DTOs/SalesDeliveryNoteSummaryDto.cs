using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesDeliveryNoteSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid? SalesOrderId,
    string? SalesOrderNumber,
    DateOnly Date,
    SalesDeliveryNoteStatus Status,
    string StatusLabel);
