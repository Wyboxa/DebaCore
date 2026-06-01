namespace Debales.Application.Purchasing.DTOs;

public sealed record PurchaseCreditNoteLineSummaryDto(
    Guid Id,
    int SortOrder,
    Guid ItemId,
    string ItemCode,
    string ItemName,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate,
    decimal LineSubtotal,
    decimal LineTaxAmount,
    decimal LineTotal);
