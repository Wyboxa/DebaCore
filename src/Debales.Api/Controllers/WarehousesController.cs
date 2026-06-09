using Debales.Application.Inventory.Commands.AddWarehouseLocation;
using Debales.Application.Inventory.Commands.CreateWarehouse;
using Debales.Application.Inventory.Queries.GetWarehouseById;
using Debales.Application.Inventory.Queries.GetWarehouses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/inventory/warehouses")]
[Authorize]
[RequiresModule("Inventory")]
public sealed class WarehousesController : ControllerBase
{
    private readonly CreateWarehouseHandler _create;
    private readonly AddWarehouseLocationHandler _addLocation;
    private readonly GetWarehousesHandler _getAll;
    private readonly GetWarehouseByIdHandler _getById;

    public WarehousesController(
        CreateWarehouseHandler create, AddWarehouseLocationHandler addLocation,
        GetWarehousesHandler getAll, GetWarehouseByIdHandler getById)
    {
        _create = create; _addLocation = addLocation;
        _getAll = getAll; _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false, CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetWarehousesQuery(activeOnly), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var w = await _getById.Handle(new GetWarehouseByIdQuery(id), ct);
        return w is null ? NotFound() : Ok(w);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseBody request, CancellationToken ct = default)
    {
        var w = await _create.Handle(new CreateWarehouseCommand(request.Code, request.Name, request.Description, "api"), ct);
        return CreatedAtAction(nameof(GetById), new { id = w.Id }, w);
    }

    [HttpPost("{id:guid}/locations")]
    public async Task<IActionResult> AddLocation(Guid id, [FromBody] AddLocationBody request, CancellationToken ct = default)
    {
        var loc = await _addLocation.Handle(new AddWarehouseLocationCommand(id, request.Code, request.Description, "api"), ct);
        return Ok(loc);
    }

    public sealed record CreateWarehouseBody(string Code, string Name, string? Description);
    public sealed record AddLocationBody(string Code, string? Description);
}
