using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseInvoiceDetailDto(
    Guid Id,
    string Number,
    string? SupplierInvoiceNumber,
    Guid SupplierId,
    string SupplierName,
    Guid? PurchaseDeliveryNoteId,
    DateOnly Date,
    DateOnly DueDate,
    PurchaseInvoiceStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<PurchaseInvoiceLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
