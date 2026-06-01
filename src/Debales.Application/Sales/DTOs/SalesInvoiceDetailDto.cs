using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesInvoiceDetailDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid? SalesDeliveryNoteId,
    DateOnly Date,
    DateOnly DueDate,
    SalesInvoiceStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<SalesInvoiceLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
