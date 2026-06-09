namespace Debales.Application.Sales.DTOs;

public sealed record PaymentTermDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    List<PaymentTermLineDto> Lines);

public sealed record PaymentTermSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    int LineCount);
