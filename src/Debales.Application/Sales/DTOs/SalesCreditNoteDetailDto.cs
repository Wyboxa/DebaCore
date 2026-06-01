using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesCreditNoteDetailDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid OriginalInvoiceId,
    string OriginalInvoiceNumber,
    DateOnly Date,
    SalesCreditNoteStatus Status,
    string StatusLabel,
    string? Reason,
    IReadOnlyList<SalesCreditNoteLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
