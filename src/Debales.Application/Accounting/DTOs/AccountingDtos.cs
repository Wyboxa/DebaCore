using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.DTOs;

public sealed record AccountSummaryDto(
    Guid Id, string Code, string Name, AccountType Type, string TypeLabel,
    bool IsPostable, bool IsActive, string? ParentCode);

public sealed record AccountDetailDto(
    Guid Id, string Code, string Name, AccountType Type, string TypeLabel,
    bool IsPostable, bool IsActive, string? ParentCode,
    DateTime CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy);

public sealed record FiscalYearSummaryDto(
    Guid Id, string Name, DateOnly StartDate, DateOnly EndDate,
    FiscalYearStatus Status, string StatusLabel, int PeriodCount);

public sealed record FiscalYearDetailDto(
    Guid Id, string Name, DateOnly StartDate, DateOnly EndDate,
    FiscalYearStatus Status, string StatusLabel,
    IReadOnlyList<FiscalPeriodDto> Periods,
    DateTime CreatedAt, string? CreatedBy);

public sealed record FiscalPeriodDto(
    Guid Id, Guid FiscalYearId, string Name,
    DateOnly StartDate, DateOnly EndDate,
    FiscalPeriodStatus Status, string StatusLabel);

public sealed record AccountingJournalDto(
    Guid Id, string Code, string Name, bool IsActive);

public sealed record AccountingEntrySummaryDto(
    Guid Id, string Number, DateOnly Date, string Description,
    Guid JournalId, string JournalCode, string JournalName,
    EntryStatus Status, string StatusLabel,
    decimal TotalDebit, decimal TotalCredit,
    string? SourceType, Guid? SourceId);

public sealed record AccountingEntryDetailDto(
    Guid Id, string Number, DateOnly Date, string Description,
    Guid JournalId, string JournalCode, string JournalName,
    Guid FiscalPeriodId, string PeriodName,
    EntryStatus Status, string StatusLabel,
    decimal TotalDebit, decimal TotalCredit, bool IsBalanced,
    string? SourceType, Guid? SourceId,
    IReadOnlyList<AccountingEntryLineDto> Lines,
    DateTime CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy);

public sealed record AccountingEntryLineDto(
    Guid Id, int SortOrder, Guid AccountId, string AccountCode,
    string Description, decimal Debit, decimal Credit,
    Guid? ThirdPartyId, string? ThirdPartyType);

public sealed record BankAccountDto(
    Guid Id, string Name, string? BankName, string? Iban, string? Bic,
    string? CurrencyCode, Guid? AccountId, string? AccountCode, string? AccountName, bool IsActive);

public sealed record BankAccountSummaryDto(
    Guid Id, string Name, string? BankName, string? Iban, string? CurrencyCode, bool IsActive);

public sealed record CashAccountSummaryDto(
    Guid Id, string Code, string Name, string CurrencyCode, decimal CurrentBalance, bool IsActive);

public sealed record CashAccountDetailDto(
    Guid Id, string Code, string Name, string CurrencyCode, decimal CurrentBalance,
    Guid? AccountId, string? AccountCode, string? AccountName, bool IsActive,
    DateTime CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy);

public sealed record RemittanceSummaryDto(
    Guid Id, string Number, DateOnly Date,
    Guid BankAccountId, string BankAccountName,
    RemittanceType Type, string TypeLabel,
    RemittanceStatus Status, string StatusLabel,
    int LineCount, decimal Total,
    DateTime CreatedAt, string? CreatedBy);

public sealed record RemittanceDetailDto(
    Guid Id, string Number, DateOnly Date,
    Guid BankAccountId, string BankAccountName,
    RemittanceType Type, string TypeLabel,
    RemittanceStatus Status, string StatusLabel,
    string? Notes, string? FailReason,
    IReadOnlyList<RemittanceLineDto> Lines, decimal Total,
    DateTime CreatedAt, string? CreatedBy, DateTime? UpdatedAt, string? UpdatedBy);

public sealed record RemittanceLineDto(Guid Id, Guid DocumentId, decimal Amount);

public sealed record TreasuryAccountDto(Guid Id, string Name, string Type, string? CurrencyCode, decimal Balance);

public sealed record TreasuryPositionDto(
    DateOnly AsOf,
    decimal TotalBankBalance,
    decimal TotalCashBalance,
    decimal TotalBalance,
    IReadOnlyList<TreasuryAccountDto> BankAccounts,
    IReadOnlyList<TreasuryAccountDto> CashAccounts);
