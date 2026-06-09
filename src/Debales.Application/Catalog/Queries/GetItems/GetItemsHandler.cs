using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Catalog.Queries.GetItems;

public sealed class GetItemsHandler
{
    private readonly IItemRepository _items;

    public GetItemsHandler(IItemRepository items) => _items = items;

    public async Task<PagedResult<ItemSummaryDto>> Handle(GetItemsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _items.SearchAsync(query.Search, query.FamilyId, query.IsService, query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(i => new ItemSummaryDto(
            i.Id, i.Code, i.Name,
            i.Family?.Name,
            i.UnitOfMeasure?.Name ?? string.Empty,
            i.IsService, i.SalePrice, i.IsActive,
            PurchasePrice: i.PurchasePrice)).ToList();

        return new PagedResult<ItemSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
