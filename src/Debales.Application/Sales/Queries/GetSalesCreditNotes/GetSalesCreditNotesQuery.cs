namespace Debales.Application.Sales.Queries.GetSalesCreditNotes;

public sealed record GetSalesCreditNotesQuery(string? Search, Guid? CustomerId, int Page, int PageSize);
