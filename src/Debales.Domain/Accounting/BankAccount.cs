using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class BankAccount : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? BankName { get; private set; }
    public string? Iban { get; private set; }
    public string? Bic { get; private set; }
    public string? CurrencyCode { get; private set; }
    public Guid? AccountId { get; private set; }
    public bool IsActive { get; private set; } = true;

    private BankAccount() { }

    public static BankAccount Create(string name, string? bankName, string? iban, string? bic,
        string? currencyCode, Guid? accountId, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        return new BankAccount
        {
            Name         = name.Trim(),
            BankName     = bankName?.Trim(),
            Iban         = NormalizeIban(iban),
            Bic          = bic?.Trim().ToUpperInvariant(),
            CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "EUR" : currencyCode.Trim().ToUpperInvariant(),
            AccountId    = accountId,
            IsActive     = true,
            CreatedBy    = createdBy
        };
    }

    public void Update(string name, string? bankName, string? iban, string? bic,
        string? currencyCode, Guid? accountId, bool isActive, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        Name         = name.Trim();
        BankName     = bankName?.Trim();
        Iban         = NormalizeIban(iban);
        Bic          = bic?.Trim().ToUpperInvariant();
        CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "EUR" : currencyCode.Trim().ToUpperInvariant();
        AccountId    = accountId;
        IsActive     = isActive;
        SetUpdated(updatedBy);
    }

    public void Delete(string deletedBy) => SoftDelete(deletedBy);

    private static string? NormalizeIban(string? iban) =>
        string.IsNullOrWhiteSpace(iban) ? null : iban.Replace(" ", "").ToUpperInvariant();
}
