using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseInvoiceSummaryDto(
    Guid Id,
    string Number,
    string? SupplierInvoiceNumber,
    Guid SupplierId,
    string SupplierName,
    DateOnly Date,
    DateOnly DueDate,
    PurchaseInvoiceStatus Status,
    string StatusLabel,
    decimal Total);
