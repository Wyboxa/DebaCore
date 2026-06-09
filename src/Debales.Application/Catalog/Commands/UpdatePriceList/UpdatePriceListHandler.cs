using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.UpdatePriceList;

public sealed class UpdatePriceListHandler
{
    private readonly IPriceListRepository _priceLists;
    private readonly IUnitOfWork _uow;

    public UpdatePriceListHandler(IPriceListRepository priceLists, IUnitOfWork uow)
    {
        _priceLists = priceLists;
        _uow = uow;
    }

    public async Task<PriceListDetailDto> Handle(
        UpdatePriceListCommand command, CancellationToken cancellationToken = default)
    {
        var pl = await _priceLists.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tarifa '{command.Id}' no encontrada.");

        pl.Update(command.Name, command.Description, command.ValidFrom, command.ValidTo, command.UpdatedBy);

        if (command.IsActive && !pl.IsActive) pl.Activate(command.UpdatedBy);
        if (!command.IsActive && pl.IsActive) pl.Deactivate(command.UpdatedBy);

        _priceLists.Update(pl);
        await _uow.SaveChangesAsync(cancellationToken);

        return ToDto(pl);
    }

    internal static PriceListDetailDto ToDto(PriceList pl) =>
        new(pl.Id, pl.Name, pl.Description, pl.IsActive,
            pl.ValidFrom, pl.ValidTo,
            pl.Items
                .OrderBy(ip => ip.Item?.Code)
                .Select(ip => new ItemPriceDto(
                    ip.ItemId,
                    ip.Item?.Code ?? "",
                    ip.Item?.Name ?? "",
                    ip.Item?.UnitOfMeasure?.Name,
                    ip.Price))
                .ToList(),
            pl.CreatedAt, pl.CreatedBy,
            pl.UpdatedAt, pl.UpdatedBy);
}
