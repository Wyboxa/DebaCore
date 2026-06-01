namespace Debales.Application.Purchasing.DTOs;

public sealed record SupplierPaymentSummaryDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid? PayableId,
    string? PayableNumber,
    DateOnly Date,
    decimal Amount,
    string? Reference);
