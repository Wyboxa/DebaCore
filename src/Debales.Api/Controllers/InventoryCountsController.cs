using Debales.Application.Inventory.Commands.AddInventoryCountLine;
using Debales.Application.Inventory.Commands.CloseInventoryCount;
using Debales.Application.Inventory.Commands.CreateInventoryCount;
using Debales.Application.Inventory.Commands.SetCountedQuantity;
using Debales.Application.Inventory.Queries.GetInventoryCountById;
using Debales.Application.Inventory.Queries.GetInventoryCounts;
using Debales.Domain.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class InventoryCountsController : ControllerBase
{
    private readonly GetInventoryCountsHandler _getAll;
    private readonly GetInventoryCountByIdHandler _getById;
    private readonly CreateInventoryCountHandler _create;
    private readonly AddInventoryCountLineHandler _addLine;
    private readonly SetCountedQuantityHandler _setQty;
    private readonly CloseInventoryCountHandler _close;

    public InventoryCountsController(
        GetInventoryCountsHandler getAll, GetInventoryCountByIdHandler getById,
        CreateInventoryCountHandler create, AddInventoryCountLineHandler addLine,
        SetCountedQuantityHandler setQty, CloseInventoryCountHandler close)
    {
        _getAll = getAll; _getById = getById; _create = create;
        _addLine = addLine; _setQty = setQty; _close = close;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? warehouseId, [FromQuery] InventoryCountStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetInventoryCountsQuery(warehouseId, status, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetInventoryCountByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInventoryCountRequest request)
    {
        var result = await _create.Handle(new CreateInventoryCountCommand(
            request.WarehouseId, request.Date, request.Notes, User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid id, [FromBody] AddInventoryCountLineRequest request)
    {
        var result = await _addLine.Handle(new AddInventoryCountLineCommand(
            id, request.ItemId, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpPut("{id:guid}/lines/{itemId:guid}")]
    public async Task<IActionResult> SetCounted(Guid id, Guid itemId, [FromBody] SetCountedQuantityRequest request)
    {
        var result = await _setQty.Handle(new SetCountedQuantityCommand(
            id, itemId, request.CountedQuantity, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpPost("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var result = await _close.Handle(new CloseInventoryCountCommand(id, User.Identity?.Name ?? "api"));
        return Ok(result);
    }
}

public sealed record CreateInventoryCountRequest(Guid WarehouseId, DateOnly Date, string? Notes);
public sealed record AddInventoryCountLineRequest(Guid ItemId);
public sealed record SetCountedQuantityRequest(decimal CountedQuantity);
