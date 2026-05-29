using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesDeliveryNoteDetailDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid? SalesOrderId,
    string? SalesOrderNumber,
    DateOnly Date,
    SalesDeliveryNoteStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<SalesDeliveryNoteLineSummaryDto> Lines,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
