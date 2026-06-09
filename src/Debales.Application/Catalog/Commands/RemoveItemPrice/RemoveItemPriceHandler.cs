using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Catalog.Commands.RemoveItemPrice;

public sealed class RemoveItemPriceHandler
{
    private readonly IPriceListRepository _priceLists;
    private readonly IUnitOfWork _uow;

    public RemoveItemPriceHandler(IPriceListRepository priceLists, IUnitOfWork uow)
    {
        _priceLists = priceLists;
        _uow = uow;
    }

    public async Task<PriceListDetailDto> Handle(
        RemoveItemPriceCommand command, CancellationToken cancellationToken = default)
    {
        var pl = await _priceLists.GetByIdAsync(command.PriceListId, cancellationToken)
            ?? throw new KeyNotFoundException($"Tarifa '{command.PriceListId}' no encontrada.");

        pl.RemoveItemPrice(command.ItemId);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _priceLists.GetByIdAsync(pl.Id, cancellationToken);
        return UpdatePriceListHandler.ToDto(saved!);
    }
}
