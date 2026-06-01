namespace Debales.Application.Purchasing.Queries.GetPurchaseCreditNotes;

public sealed record GetPurchaseCreditNotesQuery(string? Search, Guid? SupplierId, int Page, int PageSize);
