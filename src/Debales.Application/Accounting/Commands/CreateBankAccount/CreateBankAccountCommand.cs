namespace Debales.Application.Accounting.Commands.CreateBankAccount;

public sealed record CreateBankAccountCommand(
    string Name, string? BankName, string? Iban, string? Bic,
    string? CurrencyCode, Guid? AccountId, string CreatedBy);
