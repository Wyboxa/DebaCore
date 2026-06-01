using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseCreditNoteDetailDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid OriginalInvoiceId,
    string OriginalInvoiceNumber,
    DateOnly Date,
    PurchaseCreditNoteStatus Status,
    string StatusLabel,
    string? Reason,
    IReadOnlyList<PurchaseCreditNoteLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
