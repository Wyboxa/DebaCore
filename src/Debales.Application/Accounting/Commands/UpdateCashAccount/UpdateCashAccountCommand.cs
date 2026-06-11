namespace Debales.Application.Accounting.Commands.UpdateCashAccount;

public sealed record UpdateCashAccountCommand(
    Guid Id, string Code, string Name, string? CurrencyCode, Guid? AccountId, bool IsActive, string UpdatedBy);
