using Debales.Domain.Sales;

namespace Debales.Application.Sales.DTOs;

public sealed record ReceivableSummaryDto(
    Guid Id,
    string Number,
    Guid SalesInvoiceId,
    string InvoiceNumber,
    Guid CustomerId,
    string CustomerName,
    DateOnly DueDate,
    decimal OriginalAmount,
    decimal PaidAmount,
    decimal PendingAmount,
    ReceivableStatus Status,
    string StatusLabel);
