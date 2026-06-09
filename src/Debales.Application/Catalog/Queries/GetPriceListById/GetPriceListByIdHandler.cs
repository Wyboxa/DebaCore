using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.DTOs;

namespace Debales.Application.Catalog.Queries.GetPriceListById;

public sealed class GetPriceListByIdHandler
{
    private readonly IPriceListRepository _priceLists;

    public GetPriceListByIdHandler(IPriceListRepository priceLists) => _priceLists = priceLists;

    public async Task<PriceListDetailDto?> Handle(
        GetPriceListByIdQuery query, CancellationToken cancellationToken = default)
    {
        var pl = await _priceLists.GetByIdAsync(query.Id, cancellationToken);
        return pl is null ? null : UpdatePriceListHandler.ToDto(pl);
    }
}
