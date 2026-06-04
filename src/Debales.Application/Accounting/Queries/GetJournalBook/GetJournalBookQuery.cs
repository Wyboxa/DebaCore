namespace Debales.Application.Accounting.Queries.GetJournalBook;

public sealed record GetJournalBookQuery(DateOnly? From, DateOnly? To);
