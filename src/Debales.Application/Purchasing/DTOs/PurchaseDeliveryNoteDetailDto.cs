using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseDeliveryNoteDetailDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid? PurchaseOrderId,
    string? PurchaseOrderNumber,
    DateOnly Date,
    PurchaseDeliveryNoteStatus Status,
    string StatusLabel,
    string? Notes,
    IReadOnlyList<PurchaseDeliveryNoteLineSummaryDto> Lines,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy);
