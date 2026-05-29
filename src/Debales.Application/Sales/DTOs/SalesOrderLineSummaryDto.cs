namespace Debales.Application.Sales.DTOs;

public sealed record SalesOrderLineSummaryDto(
    Guid Id,
    int SortOrder,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate,
    decimal DeliveredQuantity,
    decimal PendingQuantity,
    decimal LineSubtotal,
    decimal LineTaxAmount,
    decimal LineTotal);
