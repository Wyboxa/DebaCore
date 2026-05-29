namespace Debales.Application.Sales.DTOs;

public sealed record SalesDeliveryNoteLineSummaryDto(
    Guid Id,
    int SortOrder,
    Guid? SalesOrderLineId,
    Guid? SalesOrderId,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string? Description,
    decimal Quantity);
