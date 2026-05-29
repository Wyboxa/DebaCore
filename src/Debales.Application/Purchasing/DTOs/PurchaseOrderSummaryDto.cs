using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseOrderSummaryDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    DateOnly Date,
    DateOnly? ExpectedReceiptDate,
    PurchaseOrderStatus Status,
    string StatusLabel,
    decimal Total);
