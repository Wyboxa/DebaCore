namespace Debales.Application.Accounting.Commands.CloseFiscalPeriod;

public sealed record CloseFiscalPeriodCommand(Guid PeriodId, string UpdatedBy);
