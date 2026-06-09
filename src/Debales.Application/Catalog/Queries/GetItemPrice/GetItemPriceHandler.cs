namespace Debales.Application.Catalog.Queries.GetItemPrice;

public sealed record ItemPriceResult(Guid ItemId, decimal Price, bool FromPriceList);

public sealed class GetItemPriceHandler
{
    private readonly IItemRepository _items;
    private readonly IPriceListRepository _priceLists;

    public GetItemPriceHandler(IItemRepository items, IPriceListRepository priceLists)
    {
        _items = items;
        _priceLists = priceLists;
    }

    public async Task<ItemPriceResult?> Handle(GetItemPriceQuery query, CancellationToken cancellationToken = default)
    {
        var item = await _items.GetByIdAsync(query.ItemId, cancellationToken);
        if (item is null) return null;

        if (query.PriceListId.HasValue)
        {
            var priceList = await _priceLists.GetByIdAsync(query.PriceListId.Value, cancellationToken);
            var itemPrice = priceList?.Items.FirstOrDefault(ip => ip.ItemId == query.ItemId);
            if (itemPrice is not null)
                return new ItemPriceResult(query.ItemId, itemPrice.Price, FromPriceList: true);
        }

        return new ItemPriceResult(query.ItemId, item.SalePrice, FromPriceList: false);
    }
}
