namespace Debales.Application.Accounting.Commands.CreateCashAccount;

public sealed record CreateCashAccountCommand(
    string Code, string Name, string? CurrencyCode, Guid? AccountId, string CreatedBy);
