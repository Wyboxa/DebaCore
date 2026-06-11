using Debales.Application.Catalog;
using Debales.Application.Catalog.Queries.GetItemPrice;
using Debales.Domain.Catalog;
using NSubstitute;

namespace Debales.Application.Tests.Catalog;

public sealed class GetItemPriceHandlerTests
{
    private readonly IItemRepository _items = Substitute.For<IItemRepository>();
    private readonly IPriceListRepository _priceLists = Substitute.For<IPriceListRepository>();
    private readonly GetItemPriceHandler _handler;

    public GetItemPriceHandlerTests()
    {
        _handler = new GetItemPriceHandler(_items, _priceLists);
    }

    private static Item BuildItem(decimal salePrice = 100m)
    {
        var uom = UnitOfMeasure.Create("UN", "Unidad", "system");
        return Item.Create("ART-01", "Artículo 1", null, false, uom.Id, null, null, salePrice, 60m, "system");
    }

    private static PriceList BuildPriceList(Guid itemId, decimal price)
    {
        var pl = PriceList.Create("Tarifa A", null, null, null, "system");
        pl.SetItemPrice(itemId, price, "system");
        return pl;
    }

    [Fact]
    public async Task Handle_ItemNotFound_ReturnsNull()
    {
        _items.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Item?)null);

        var result = await _handler.Handle(new GetItemPriceQuery(Guid.NewGuid(), null));

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_NoPriceListId_ReturnsSalePrice()
    {
        var item = BuildItem(salePrice: 150m);
        _items.GetByIdAsync(item.Id, Arg.Any<CancellationToken>()).Returns(item);

        var result = await _handler.Handle(new GetItemPriceQuery(item.Id, null));

        Assert.NotNull(result);
        Assert.Equal(150m, result.Price);
        Assert.False(result.FromPriceList);
    }

    [Fact]
    public async Task Handle_WithPriceList_ItemHasEntry_ReturnsPriceListPrice()
    {
        var item = BuildItem(salePrice: 150m);
        _items.GetByIdAsync(item.Id, Arg.Any<CancellationToken>()).Returns(item);

        var priceListId = Guid.NewGuid();
        var priceList = BuildPriceList(item.Id, 99m);
        _priceLists.GetByIdAsync(priceListId, Arg.Any<CancellationToken>()).Returns(priceList);

        var result = await _handler.Handle(new GetItemPriceQuery(item.Id, priceListId));

        Assert.NotNull(result);
        Assert.Equal(99m, result.Price);
        Assert.True(result.FromPriceList);
    }

    [Fact]
    public async Task Handle_WithPriceList_ItemNotInList_FallsBackToSalePrice()
    {
        var item = BuildItem(salePrice: 150m);
        _items.GetByIdAsync(item.Id, Arg.Any<CancellationToken>()).Returns(item);

        var priceListId = Guid.NewGuid();
        var priceList = BuildPriceList(Guid.NewGuid(), 50m); // different item
        _priceLists.GetByIdAsync(priceListId, Arg.Any<CancellationToken>()).Returns(priceList);

        var result = await _handler.Handle(new GetItemPriceQuery(item.Id, priceListId));

        Assert.NotNull(result);
        Assert.Equal(150m, result.Price);
        Assert.False(result.FromPriceList);
    }

    [Fact]
    public async Task Handle_WithPriceList_PriceListNotFound_FallsBackToSalePrice()
    {
        var item = BuildItem(salePrice: 200m);
        _items.GetByIdAsync(item.Id, Arg.Any<CancellationToken>()).Returns(item);
        _priceLists.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((PriceList?)null);

        var result = await _handler.Handle(new GetItemPriceQuery(item.Id, Guid.NewGuid()));

        Assert.NotNull(result);
        Assert.Equal(200m, result.Price);
        Assert.False(result.FromPriceList);
    }

    [Fact]
    public async Task Handle_ItemIdReturnedInResult()
    {
        var item = BuildItem();
        _items.GetByIdAsync(item.Id, Arg.Any<CancellationToken>()).Returns(item);

        var result = await _handler.Handle(new GetItemPriceQuery(item.Id, null));

        Assert.NotNull(result);
        Assert.Equal(item.Id, result.ItemId);
    }
}
