using Debales.Application.Inventory.Commands.CreateStockMovement;
using Debales.Application.Inventory.Queries.GetStockBalance;
using Debales.Application.Inventory.Queries.GetStockMovementById;
using Debales.Application.Inventory.Queries.GetStockMovements;
using Debales.Domain.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public sealed class StockMovementsController : ControllerBase
{
    private readonly CreateStockMovementHandler _create;
    private readonly GetStockMovementsHandler _getAll;
    private readonly GetStockMovementByIdHandler _getById;
    private readonly GetStockBalanceHandler _getBalance;

    public StockMovementsController(
        CreateStockMovementHandler create,
        GetStockMovementsHandler getAll, GetStockMovementByIdHandler getById,
        GetStockBalanceHandler getBalance)
    {
        _create = create; _getAll = getAll; _getById = getById; _getBalance = getBalance;
    }

    [HttpGet("movements")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? itemId, [FromQuery] Guid? warehouseId,
        [FromQuery] StockMovementType? type,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetStockMovementsQuery(search, itemId, warehouseId, type, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("movements/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var m = await _getById.Handle(new GetStockMovementByIdQuery(id), ct);
        return m is null ? NotFound() : Ok(m);
    }

    [HttpPost("movements")]
    public async Task<IActionResult> Create([FromBody] StockMovementBody request, CancellationToken ct = default)
    {
        var m = await _create.Handle(
            new CreateStockMovementCommand(
                request.Type, request.ItemId, request.WarehouseId, request.LocationId,
                request.Date, request.Quantity, request.Reference, request.Notes, "api"), ct);
        return Ok(m);
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(
        [FromQuery] Guid? itemId, [FromQuery] Guid? warehouseId,
        CancellationToken ct = default)
    {
        var result = await _getBalance.Handle(new GetStockBalanceQuery(itemId, warehouseId), ct);
        return Ok(result);
    }

    public sealed record StockMovementBody(
        StockMovementType Type, Guid ItemId, Guid WarehouseId, Guid? LocationId,
        DateOnly Date, decimal Quantity, string? Reference, string? Notes);
}
