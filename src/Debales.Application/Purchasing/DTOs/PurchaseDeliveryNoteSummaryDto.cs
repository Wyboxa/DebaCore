using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseDeliveryNoteSummaryDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid? PurchaseOrderId,
    string? PurchaseOrderNumber,
    DateOnly Date,
    PurchaseDeliveryNoteStatus Status,
    string StatusLabel);
