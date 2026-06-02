using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class AccountingJournal : AuditableEntity
{
    private AccountingJournal() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public static AccountingJournal Create(string code, string name, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AccountingJournal
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        SetUpdated(updatedBy);
    }

    internal static AccountingJournal ForSeed(Guid id, string code, string name) =>
        new()
        {
            Id = id, Code = code, Name = name, IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system"
        };
}
