namespace Debales.Application.Sales.DTOs;

public sealed record CustomerPaymentSummaryDto(
    Guid Id,
    string Number,
    Guid CustomerId,
    string CustomerName,
    Guid? ReceivableId,
    string? ReceivableNumber,
    DateOnly Date,
    decimal Amount,
    string? Reference);
