using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseOrderDetailDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    DateOnly Date,
    DateOnly? ExpectedReceiptDate,
    PurchaseOrderStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<PurchaseOrderLineSummaryDto> Lines,
    decimal Subtotal,
    decimal TaxAmount,
    decimal Total,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
