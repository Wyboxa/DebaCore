namespace Debales.Application.Common;

public sealed record StatementLineDto(
    DateOnly Date,
    string Type,
    string Reference,
    string Description,
    decimal Debit,
    decimal Credit,
    decimal Balance);

public sealed record StatementDto(
    Guid ThirdPartyId,
    string ThirdPartyName,
    DateOnly? From,
    DateOnly? To,
    decimal TotalDebit,
    decimal TotalCredit,
    decimal Balance,
    IReadOnlyList<StatementLineDto> Lines);
