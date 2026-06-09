using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Catalog.Queries.GetPriceLists;

public sealed class GetPriceListsHandler
{
    private readonly IPriceListRepository _priceLists;

    public GetPriceListsHandler(IPriceListRepository priceLists) => _priceLists = priceLists;

    public async Task<PagedResult<PriceListSummaryDto>> Handle(
        GetPriceListsQuery query, CancellationToken cancellationToken = default)
    {
        var paged = await _priceLists.SearchAsync(
            query.Search, query.IsActive, query.Page, query.PageSize, cancellationToken);

        var dtos = paged.Items
            .Select(pl => new PriceListSummaryDto(
                pl.Id, pl.Name, pl.Description, pl.IsActive,
                pl.ValidFrom, pl.ValidTo, pl.Items.Count,
                pl.CreatedAt, pl.CreatedBy))
            .ToList();

        return new PagedResult<PriceListSummaryDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
