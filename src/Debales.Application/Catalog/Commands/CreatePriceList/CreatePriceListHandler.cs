using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.CreatePriceList;

public sealed class CreatePriceListHandler
{
    private readonly IPriceListRepository _priceLists;
    private readonly IUnitOfWork _uow;

    public CreatePriceListHandler(IPriceListRepository priceLists, IUnitOfWork uow)
    {
        _priceLists = priceLists;
        _uow = uow;
    }

    public async Task<PriceListDetailDto> Handle(
        CreatePriceListCommand command, CancellationToken cancellationToken = default)
    {
        var pl = PriceList.Create(
            command.Name, command.Description,
            command.ValidFrom, command.ValidTo, command.CreatedBy);

        await _priceLists.AddAsync(pl, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _priceLists.GetByIdAsync(pl.Id, cancellationToken);
        return UpdatePriceListHandler.ToDto(saved!);
    }
}
