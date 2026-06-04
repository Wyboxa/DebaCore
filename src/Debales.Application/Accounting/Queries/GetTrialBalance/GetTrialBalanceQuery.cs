namespace Debales.Application.Accounting.Queries.GetTrialBalance;

public sealed record GetTrialBalanceQuery(DateOnly? From, DateOnly? To);
