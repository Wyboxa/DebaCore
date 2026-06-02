using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class Account : AuditableEntity
{
    private Account() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public AccountType Type { get; private set; }
    public bool IsPostable { get; private set; }
    public bool IsActive { get; private set; }
    public string? ParentCode { get; private set; }

    public static Account Create(
        string code, string name, AccountType type,
        bool isPostable, string? parentCode, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Account
        {
            Code = code.Trim(),
            Name = name.Trim(),
            Type = type,
            IsPostable = isPostable,
            IsActive = true,
            ParentCode = parentCode?.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Update(string name, AccountType type, bool isPostable, string? parentCode, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        Type = type;
        IsPostable = isPostable;
        ParentCode = parentCode?.Trim();
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    internal static Account ForSeed(
        Guid id, string code, string name, AccountType type, bool isPostable, string? parentCode) =>
        new()
        {
            Id = id, Code = code, Name = name, Type = type,
            IsPostable = isPostable, IsActive = true, ParentCode = parentCode,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system"
        };
}
