namespace Debales.Application.Accounting.Commands.CreateAccountingEntry;

public sealed record CreateAccountingEntryCommand(
    DateOnly Date, string Description, Guid JournalId, Guid FiscalPeriodId,
    IReadOnlyList<CreateEntryLineDto> Lines, string CreatedBy);

public sealed record CreateEntryLineDto(
    Guid AccountId, string AccountCode, string Description,
    decimal Debit, decimal Credit,
    Guid? ThirdPartyId, string? ThirdPartyType);
