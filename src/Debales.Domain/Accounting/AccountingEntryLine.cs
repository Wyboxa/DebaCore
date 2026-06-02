using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class AccountingEntryLine : Entity
{
    private AccountingEntryLine() { }

    public Guid AccountingEntryId { get; private set; }
    public int SortOrder { get; private set; }
    public Guid AccountId { get; private set; }
    public string AccountCode { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }
    public Guid? ThirdPartyId { get; private set; }
    public string? ThirdPartyType { get; private set; }

    internal static AccountingEntryLine Create(
        Guid entryId, int sortOrder,
        Guid accountId, string accountCode, string description,
        decimal debit, decimal credit,
        Guid? thirdPartyId, string? thirdPartyType)
    {
        return new AccountingEntryLine
        {
            AccountingEntryId = entryId,
            SortOrder = sortOrder,
            AccountId = accountId,
            AccountCode = accountCode,
            Description = description.Trim(),
            Debit = debit,
            Credit = credit,
            ThirdPartyId = thirdPartyId,
            ThirdPartyType = thirdPartyType
        };
    }
}
