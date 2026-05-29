namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseDeliveryNoteLineSummaryDto(
    Guid Id,
    int SortOrder,
    Guid? PurchaseOrderLineId,
    Guid? PurchaseOrderId,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string? Description,
    decimal Quantity);
