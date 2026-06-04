using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.DTOs;

public sealed record TrialBalanceLineDto(
    string AccountCode,
    string AccountName,
    AccountType AccountType,
    decimal TotalDebit,
    decimal TotalCredit,
    decimal Balance);

public sealed record TrialBalanceDto(
    string FiscalYearName,
    DateOnly? From,
    DateOnly? To,
    IReadOnlyList<TrialBalanceLineDto> Lines,
    decimal GrandTotalDebit,
    decimal GrandTotalCredit);

public sealed record JournalBookEntryDto(
    string Number,
    DateOnly Date,
    string JournalCode,
    string Description,
    EntryStatus Status,
    IReadOnlyList<JournalBookLineDto> Lines,
    decimal TotalDebit,
    decimal TotalCredit);

public sealed record JournalBookLineDto(
    string AccountCode,
    string Description,
    decimal Debit,
    decimal Credit);

public sealed record BalanceSheetGroupDto(
    AccountType Type,
    string TypeLabel,
    IReadOnlyList<BalanceSheetLineDto> Lines,
    decimal Total);

public sealed record BalanceSheetLineDto(
    string AccountCode,
    string AccountName,
    decimal Balance);

public sealed record BalanceSheetDto(
    DateOnly? To,
    IReadOnlyList<BalanceSheetGroupDto> Groups,
    decimal TotalAssets,
    decimal TotalLiabilitiesAndEquity,
    bool IsBalanced);
