using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.DTOs;

namespace Debales.Application.Catalog.Queries.GetItemById;

public sealed class GetItemByIdHandler
{
    private readonly IItemRepository _items;

    public GetItemByIdHandler(IItemRepository items) => _items = items;

    public async Task<ItemDetailDto?> Handle(GetItemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var item = await _items.GetByIdAsync(query.Id, cancellationToken);
        return item is null ? null : UpdateItemHandler.ToDto(item);
    }
}
