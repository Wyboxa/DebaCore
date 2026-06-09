using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Catalog.Commands.SetItemPrice;

public sealed class SetItemPriceHandler
{
    private readonly IPriceListRepository _priceLists;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public SetItemPriceHandler(IPriceListRepository priceLists, IItemRepository items, IUnitOfWork uow)
    {
        _priceLists = priceLists;
        _items = items;
        _uow = uow;
    }

    public async Task<PriceListDetailDto> Handle(
        SetItemPriceCommand command, CancellationToken cancellationToken = default)
    {
        var pl = await _priceLists.GetByIdAsync(command.PriceListId, cancellationToken)
            ?? throw new KeyNotFoundException($"Tarifa '{command.PriceListId}' no encontrada.");

        var item = await _items.GetByIdAsync(command.ItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Artículo '{command.ItemId}' no encontrado.");

        pl.SetItemPrice(item.Id, command.Price, command.UpdatedBy);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _priceLists.GetByIdAsync(pl.Id, cancellationToken);
        return UpdatePriceListHandler.ToDto(saved!);
    }
}
