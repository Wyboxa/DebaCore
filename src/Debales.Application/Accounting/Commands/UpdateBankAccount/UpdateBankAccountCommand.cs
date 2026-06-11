namespace Debales.Application.Accounting.Commands.UpdateBankAccount;

public sealed record UpdateBankAccountCommand(
    Guid Id, string Name, string? BankName, string? Iban, string? Bic,
    string? CurrencyCode, Guid? AccountId, bool IsActive, string UpdatedBy);
