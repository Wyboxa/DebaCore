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
