namespace Debales.Application.Catalog.Queries.GetItems;

public sealed record GetItemsQuery(string? Search, Guid? FamilyId, bool? IsService, int Page, int PageSize);
