using Debales.Application.Catalog.Commands.CreateItem;
using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.Queries.GetCatalogLookups;
using Debales.Application.Catalog.Queries.GetItemById;
using Debales.Application.Catalog.Queries.GetItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ItemsController : ControllerBase
{
    private readonly CreateItemHandler _create;
    private readonly UpdateItemHandler _update;
    private readonly GetItemByIdHandler _getById;
    private readonly GetItemsHandler _getAll;
    private readonly GetCatalogLookupsHandler _lookups;

    public ItemsController(
        CreateItemHandler create,
        UpdateItemHandler update,
        GetItemByIdHandler getById,
        GetItemsHandler getAll,
        GetCatalogLookupsHandler lookups)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getAll = getAll;
        _lookups = lookups;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? familyId,
        [FromQuery] bool? isService,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetItemsQuery(search, familyId, isService, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var item = await _getById.Handle(new GetItemByIdQuery(id), ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookups(CancellationToken ct = default)
    {
        var result = await _lookups.Handle(ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request, CancellationToken ct = default)
    {
        var item = await _create.Handle(
            new CreateItemCommand(
                request.Code, request.Name, request.Description,
                request.IsService, request.UnitOfMeasureId, request.FamilyId, request.TaxTypeId,
                request.SalePrice, request.PurchasePrice, "api", request.MinimumStock), ct);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemRequest request, CancellationToken ct = default)
    {
        var item = await _update.Handle(
            new UpdateItemCommand(
                id, request.Code, request.Name, request.Description,
                request.IsService, request.UnitOfMeasureId, request.FamilyId, request.TaxTypeId,
                request.SalePrice, request.PurchasePrice, "api", request.MinimumStock), ct);
        return Ok(item);
    }

    public sealed record CreateItemRequest(
        string Code, string Name, string? Description,
        bool IsService, Guid UnitOfMeasureId, Guid? FamilyId, Guid? TaxTypeId,
        decimal SalePrice, decimal PurchasePrice, decimal? MinimumStock = null);

    public sealed record UpdateItemRequest(
        string Code, string Name, string? Description,
        bool IsService, Guid UnitOfMeasureId, Guid? FamilyId, Guid? TaxTypeId,
        decimal SalePrice, decimal PurchasePrice, decimal? MinimumStock = null);
}
