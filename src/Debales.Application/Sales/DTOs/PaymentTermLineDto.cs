namespace Debales.Application.Sales.DTOs;

public sealed record PaymentTermLineDto(int DueDays, decimal Percentage, int SortOrder);
