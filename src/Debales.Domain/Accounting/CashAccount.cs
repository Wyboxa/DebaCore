using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class CashAccount : AuditableEntity
{
    private CashAccount() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string CurrencyCode { get; private set; } = "EUR";
    public decimal CurrentBalance { get; private set; }
    public Guid? AccountId { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Account? Account { get; private set; }

    public static CashAccount Create(string code, string name, string? currencyCode, Guid? accountId, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new CashAccount
        {
            Code         = code.Trim().ToUpperInvariant(),
            Name         = name.Trim(),
            CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "EUR" : currencyCode.Trim().ToUpperInvariant(),
            AccountId    = accountId,
            IsActive     = true,
            CreatedBy    = createdBy
        };
    }

    public void Update(string code, string name, string? currencyCode, Guid? accountId, bool isActive, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Code         = code.Trim().ToUpperInvariant();
        Name         = name.Trim();
        CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "EUR" : currencyCode.Trim().ToUpperInvariant();
        AccountId    = accountId;
        IsActive     = isActive;
        SetUpdated(updatedBy);
    }

    public void AdjustBalance(decimal amount, string updatedBy)
    {
        CurrentBalance += amount;
        SetUpdated(updatedBy);
    }

    public void Delete(string deletedBy) => SoftDelete(deletedBy);
}
