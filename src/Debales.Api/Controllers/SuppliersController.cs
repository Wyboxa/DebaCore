using Debales.Application.Catalog.Commands.DeleteSupplierItemCode;
using Debales.Application.Catalog.Commands.UpsertSupplierItemCode;
using Debales.Application.Catalog.Queries.GetSupplierItemCodes;
using Debales.Application.Suppliers.Commands.CreateSupplier;
using Debales.Application.Suppliers.Commands.UpdateSupplier;
using Debales.Application.Suppliers.Queries.GetSupplierById;
using Debales.Application.Suppliers.Queries.GetSuppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SuppliersController : ControllerBase
{
    private readonly CreateSupplierHandler _create;
    private readonly UpdateSupplierHandler _update;
    private readonly GetSupplierByIdHandler _getById;
    private readonly GetSuppliersHandler _getAll;
    private readonly GetSupplierItemCodesHandler _getItemCodes;
    private readonly UpsertSupplierItemCodeHandler _upsertItemCode;
    private readonly DeleteSupplierItemCodeHandler _deleteItemCode;

    public SuppliersController(
        CreateSupplierHandler create,
        UpdateSupplierHandler update,
        GetSupplierByIdHandler getById,
        GetSuppliersHandler getAll,
        GetSupplierItemCodesHandler getItemCodes,
        UpsertSupplierItemCodeHandler upsertItemCode,
        DeleteSupplierItemCodeHandler deleteItemCode)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getAll = getAll;
        _getItemCodes = getItemCodes;
        _upsertItemCode = upsertItemCode;
        _deleteItemCode = deleteItemCode;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSuppliersQuery(search, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var supplier = await _getById.Handle(new GetSupplierByIdQuery(id), ct);
        return supplier is null ? NotFound() : Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request, CancellationToken ct = default)
    {
        var supplier = await _create.Handle(
            new CreateSupplierCommand(request.Name, request.TaxId, request.Phone, request.Email, request.ContactName, "api"), ct);
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest request, CancellationToken ct = default)
    {
        var supplier = await _update.Handle(
            new UpdateSupplierCommand(
                id, request.Name, request.TaxId, request.Phone, request.Email,
                request.Website, request.ContactName, request.Notes,
                request.AddressStreet, request.AddressCity,
                request.AddressPostalCode, request.AddressCountry, "api"), ct);
        return Ok(supplier);
    }

    public sealed record CreateSupplierRequest(
        string Name, string? TaxId, string? Phone, string? Email, string? ContactName);

    public sealed record UpdateSupplierRequest(
        string Name, string? TaxId, string? Phone, string? Email, string? Website,
        string? ContactName, string? Notes,
        string? AddressStreet, string? AddressCity, string? AddressPostalCode, string? AddressCountry);

    // ── Item codes ────────────────────────────────────────────────────────────

    [HttpGet("{id:guid}/item-codes")]
    public async Task<IActionResult> GetItemCodes(Guid id, CancellationToken ct)
        => Ok(await _getItemCodes.Handle(new GetSupplierItemCodesQuery(id), ct));

    [HttpPost("{id:guid}/item-codes")]
    public async Task<IActionResult> UpsertItemCode(Guid id, [FromBody] UpsertSupplierItemCodeRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _upsertItemCode.Handle(
                new UpsertSupplierItemCodeCommand(id, req.ItemId, req.SupplierCode, req.Description, "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:guid}/item-codes/{itemId:guid}")]
    public async Task<IActionResult> DeleteItemCode(Guid id, Guid itemId, CancellationToken ct)
    {
        try
        {
            await _deleteItemCode.Handle(new DeleteSupplierItemCodeCommand(id, itemId), ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    public sealed record UpsertSupplierItemCodeRequest(Guid ItemId, string SupplierCode, string? Description);
}
