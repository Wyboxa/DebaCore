using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class AccountingTemplate : AuditableEntity
{
    private readonly List<AccountingTemplateLine> _lines = [];
    private AccountingTemplate() { }

    public string Code { get; private set; } = null!;
    public string EventType { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string JournalCode { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public IReadOnlyList<AccountingTemplateLine> Lines => _lines.AsReadOnly();

    public static AccountingTemplate Create(
        string code, string eventType, string name, string journalCode, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(journalCode);

        return new AccountingTemplate
        {
            Code = code.Trim().ToUpper(),
            EventType = eventType.Trim(),
            Name = name.Trim(),
            JournalCode = journalCode.Trim().ToUpper(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public AccountingTemplateLine AddLine(
        int sortOrder, TemplateSide side, string accountCode,
        TemplateAmountType amountType, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountCode);
        var line = AccountingTemplateLine.Create(Id, sortOrder, side, accountCode, amountType, description);
        _lines.Add(line);
        return line;
    }

    internal static AccountingTemplate ForSeed(
        Guid id, string code, string eventType, string name, string journalCode) =>
        new()
        {
            Id = id, Code = code, EventType = eventType, Name = name,
            JournalCode = journalCode, IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system"
        };
}
