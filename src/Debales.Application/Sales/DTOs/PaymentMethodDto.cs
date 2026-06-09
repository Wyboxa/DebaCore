namespace Debales.Application.Sales.DTOs;

public sealed record PaymentMethodDto(Guid Id, string Name, string? Code, string? Description, bool IsActive);
public sealed record PaymentMethodSummaryDto(Guid Id, string Name, string? Code, string? Description, bool IsActive);
