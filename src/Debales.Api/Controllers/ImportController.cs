using Debales.Application.Catalog.Commands.ImportItems;
using Debales.Application.CRM.Customers.Commands.ImportCustomers;
using Debales.Application.Suppliers.Commands.ImportSuppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/import")]
[Authorize]
public sealed class ImportController : ControllerBase
{
    private readonly ImportCustomersHandler _customers;
    private readonly ImportSuppliersHandler _suppliers;
    private readonly ImportItemsHandler _items;

    public ImportController(
        ImportCustomersHandler customers,
        ImportSuppliersHandler suppliers,
        ImportItemsHandler items)
    {
        _customers = customers;
        _suppliers = suppliers;
        _items = items;
    }

    [HttpPost("customers")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportCustomers(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "El archivo CSV está vacío." });
        using var reader = new StreamReader(file.OpenReadStream());
        var csv = await reader.ReadToEndAsync(ct);
        var result = await _customers.Handle(new ImportCustomersCommand(csv, User.Identity?.Name ?? "api"), ct);
        return Ok(result);
    }

    [HttpPost("suppliers")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportSuppliers(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "El archivo CSV está vacío." });
        using var reader = new StreamReader(file.OpenReadStream());
        var csv = await reader.ReadToEndAsync(ct);
        var result = await _suppliers.Handle(new ImportSuppliersCommand(csv, User.Identity?.Name ?? "api"), ct);
        return Ok(result);
    }

    [HttpPost("items")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportItems(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "El archivo CSV está vacío." });
        using var reader = new StreamReader(file.OpenReadStream());
        var csv = await reader.ReadToEndAsync(ct);
        var result = await _items.Handle(new ImportItemsCommand(csv, User.Identity?.Name ?? "api"), ct);
        return Ok(result);
    }
}
