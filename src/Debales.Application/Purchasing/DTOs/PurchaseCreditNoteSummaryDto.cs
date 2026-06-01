using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseCreditNoteSummaryDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid OriginalInvoiceId,
    string OriginalInvoiceNumber,
    DateOnly Date,
    PurchaseCreditNoteStatus Status,
    string StatusLabel,
    decimal Total);
