using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PayableSummaryDto(
    Guid Id,
    string Number,
    Guid PurchaseInvoiceId,
    string InvoiceNumber,
    Guid SupplierId,
    string SupplierName,
    DateOnly DueDate,
    decimal OriginalAmount,
    decimal PaidAmount,
    decimal PendingAmount,
    PayableStatus Status,
    string StatusLabel);
