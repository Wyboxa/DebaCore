using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesOrderDetailDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    DateOnly Date,
    DateOnly? RequestedDeliveryDate,
    SalesOrderStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<SalesOrderLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
