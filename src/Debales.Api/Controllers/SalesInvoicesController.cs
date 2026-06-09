using Debales.Application.Documents;
using Debales.Application.Sales.Commands.CancelSalesInvoice;
using Debales.Application.Sales.Commands.CreateSalesInvoice;
using Debales.Application.Sales.Commands.PostSalesInvoice;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Application.Sales.Queries.GetSalesInvoices;
using Debales.Domain.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales/invoices")]
[Authorize]
[RequiresModule("Sales")]
public sealed class SalesInvoicesController : ControllerBase
{
    private readonly CreateSalesInvoiceHandler _create;
    private readonly PostSalesInvoiceHandler _post;
    private readonly CancelSalesInvoiceHandler _cancel;
    private readonly GetSalesInvoicesHandler _getAll;
    private readonly GetSalesInvoiceByIdHandler _getById;
    private readonly IInvoicePdfGenerator _pdf;

    public SalesInvoicesController(
        CreateSalesInvoiceHandler create, PostSalesInvoiceHandler post,
        CancelSalesInvoiceHandler cancel,
        GetSalesInvoicesHandler getAll, GetSalesInvoiceByIdHandler getById,
        IInvoicePdfGenerator pdf)
    {
        _create = create; _post = post; _cancel = cancel;
        _getAll = getAll; _getById = getById; _pdf = pdf;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? customerId,
        [FromQuery] SalesInvoiceStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSalesInvoicesQuery(search, customerId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var invoice = await _getById.Handle(new GetSalesInvoiceByIdQuery(id), ct);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesInvoiceBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreateSalesInvoiceLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var invoice = await _create.Handle(
            new CreateSalesInvoiceCommand(
                request.CustomerId, request.SalesDeliveryNoteId,
                request.Date, request.DueDate, request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var invoice = await _post.Handle(new PostSalesInvoiceCommand(id, "api"), ct);
        return Ok(invoice);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
    {
        await _cancel.Handle(new CancelSalesInvoiceCommand(id, "api"), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}/pdf")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPdf(Guid id, CancellationToken ct = default)
    {
        var invoice = await _getById.Handle(new GetSalesInvoiceByIdQuery(id), ct);
        if (invoice is null) return NotFound();
        var bytes = _pdf.GenerateSalesInvoice(invoice);
        return File(bytes, "application/pdf", $"Factura-{invoice.Number}.pdf");
    }

    public sealed record SalesInvoiceLineBody(Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record SalesInvoiceBody(
        Guid CustomerId, Guid? SalesDeliveryNoteId,
        DateOnly Date, DateOnly DueDate, string? Notes,
        IReadOnlyList<SalesInvoiceLineBody> Lines);
}
