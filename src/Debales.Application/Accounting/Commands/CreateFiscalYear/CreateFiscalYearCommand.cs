namespace Debales.Application.Accounting.Commands.CreateFiscalYear;

public sealed record CreateFiscalYearCommand(
    string Name, DateOnly StartDate, DateOnly EndDate, string CreatedBy);
