using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class AccountingTemplateLine : Entity
{
    private AccountingTemplateLine() { }

    public Guid AccountingTemplateId { get; private set; }
    public int SortOrder { get; private set; }
    public TemplateSide Side { get; private set; }
    public string AccountCode { get; private set; } = null!;
    public TemplateAmountType AmountType { get; private set; }
    public string Description { get; private set; } = null!;

    internal static AccountingTemplateLine Create(
        Guid templateId, int sortOrder,
        TemplateSide side, string accountCode,
        TemplateAmountType amountType, string description)
    {
        return new AccountingTemplateLine
        {
            AccountingTemplateId = templateId,
            SortOrder = sortOrder,
            Side = side,
            AccountCode = accountCode.Trim(),
            AmountType = amountType,
            Description = description.Trim()
        };
    }

    internal static AccountingTemplateLine ForSeed(
        Guid id, Guid templateId, int sortOrder,
        TemplateSide side, string accountCode,
        TemplateAmountType amountType, string description) =>
        new()
        {
            Id = id, AccountingTemplateId = templateId, SortOrder = sortOrder,
            Side = side, AccountCode = accountCode, AmountType = amountType,
            Description = description,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
}
