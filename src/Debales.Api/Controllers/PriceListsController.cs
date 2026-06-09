using Debales.Application.Catalog.Commands.CreatePriceList;
using Debales.Application.Catalog.Commands.RemoveItemPrice;
using Debales.Application.Catalog.Commands.SetItemPrice;
using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.Queries.GetPriceListById;
using Debales.Application.Catalog.Queries.GetPriceLists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PriceListsController : ControllerBase
{
    private readonly GetPriceListsHandler _getAll;
    private readonly GetPriceListByIdHandler _getById;
    private readonly CreatePriceListHandler _create;
    private readonly UpdatePriceListHandler _update;
    private readonly SetItemPriceHandler _setItemPrice;
    private readonly RemoveItemPriceHandler _removeItemPrice;

    public PriceListsController(
        GetPriceListsHandler getAll,
        GetPriceListByIdHandler getById,
        CreatePriceListHandler create,
        UpdatePriceListHandler update,
        SetItemPriceHandler setItemPrice,
        RemoveItemPriceHandler removeItemPrice)
    {
        _getAll = getAll;
        _getById = getById;
        _create = create;
        _update = update;
        _setItemPrice = setItemPrice;
        _removeItemPrice = removeItemPrice;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetPriceListsQuery(search, isActive, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await _getById.Handle(new GetPriceListByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePriceListRequest request, CancellationToken ct = default)
    {
        var result = await _create.Handle(
            new CreatePriceListCommand(request.Name, request.Description, request.ValidFrom, request.ValidTo, "api"),
            ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePriceListRequest request, CancellationToken ct = default)
    {
        var result = await _update.Handle(
            new UpdatePriceListCommand(id, request.Name, request.Description,
                request.ValidFrom, request.ValidTo, request.IsActive, "api"),
            ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> SetItemPrice(
        Guid id, [FromBody] SetItemPriceRequest request, CancellationToken ct = default)
    {
        var result = await _setItemPrice.Handle(
            new SetItemPriceCommand(id, request.ItemId, request.Price, "api"),
            ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItemPrice(
        Guid id, Guid itemId, CancellationToken ct = default)
    {
        var result = await _removeItemPrice.Handle(
            new RemoveItemPriceCommand(id, itemId, "api"),
            ct);
        return Ok(result);
    }

    public sealed record CreatePriceListRequest(
        string Name,
        string? Description,
        DateOnly? ValidFrom,
        DateOnly? ValidTo);

    public sealed record UpdatePriceListRequest(
        string Name,
        string? Description,
        DateOnly? ValidFrom,
        DateOnly? ValidTo,
        bool IsActive);

    public sealed record SetItemPriceRequest(Guid ItemId, decimal Price);
}
