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

    public SuppliersController(
        CreateSupplierHandler create,
        UpdateSupplierHandler update,
        GetSupplierByIdHandler getById,
        GetSuppliersHandler getAll)
    {
        _create = create;
        _update = update;
        _getById = getById;
        _getAll = getAll;
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
}
