namespace Debales.Application.Catalog.Queries.GetPriceLists;

public sealed record GetPriceListsQuery(
    string? Search,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20);
