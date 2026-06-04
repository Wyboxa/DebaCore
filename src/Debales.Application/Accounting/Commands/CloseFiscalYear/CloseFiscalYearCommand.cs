namespace Debales.Application.Accounting.Commands.CloseFiscalYear;

public sealed record CloseFiscalYearCommand(Guid YearId, string UpdatedBy);
