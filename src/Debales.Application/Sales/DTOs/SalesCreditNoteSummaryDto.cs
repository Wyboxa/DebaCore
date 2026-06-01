using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record SalesCreditNoteSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid OriginalInvoiceId,
    string OriginalInvoiceNumber,
    DateOnly Date,
    SalesCreditNoteStatus Status,
    string StatusLabel,
    decimal Total);
