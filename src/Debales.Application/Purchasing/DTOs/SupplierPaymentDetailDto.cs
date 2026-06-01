namespace Debales.Application.Purchasing.DTOs;

public sealed record SupplierPaymentDetailDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    Guid? PayableId,
    string? PayableNumber,
    DateOnly Date,
    decimal Amount,
    string? Reference,
    string? Notes,
    DateTime CreatedAt,
    string? CreatedBy);
