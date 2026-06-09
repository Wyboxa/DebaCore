using Debales.Application.Documents;
using Debales.Application.Purchasing.Commands.CancelPurchaseInvoice;
using Debales.Application.Purchasing.Commands.CreatePurchaseInvoice;
using Debales.Application.Purchasing.Commands.PostPurchaseInvoice;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoices;
using Debales.Domain.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/purchasing/invoices")]
[Authorize]
[RequiresModule("Purchasing")]
public sealed class PurchaseInvoicesController : ControllerBase
{
    private readonly CreatePurchaseInvoiceHandler _create;
    private readonly PostPurchaseInvoiceHandler _post;
    private readonly CancelPurchaseInvoiceHandler _cancel;
    private readonly GetPurchaseInvoicesHandler _getAll;
    private readonly GetPurchaseInvoiceByIdHandler _getById;
    private readonly IInvoicePdfGenerator _pdf;

    public PurchaseInvoicesController(
        CreatePurchaseInvoiceHandler create, PostPurchaseInvoiceHandler post,
        CancelPurchaseInvoiceHandler cancel,
        GetPurchaseInvoicesHandler getAll, GetPurchaseInvoiceByIdHandler getById,
        IInvoicePdfGenerator pdf)
    {
        _create = create; _post = post; _cancel = cancel;
        _getAll = getAll; _getById = getById; _pdf = pdf;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? supplierId,
        [FromQuery] PurchaseInvoiceStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetPurchaseInvoicesQuery(search, supplierId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var invoice = await _getById.Handle(new GetPurchaseInvoiceByIdQuery(id), ct);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseInvoiceBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreatePurchaseInvoiceLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var invoice = await _create.Handle(
            new CreatePurchaseInvoiceCommand(
                request.SupplierInvoiceNumber, request.SupplierId,
                request.PurchaseDeliveryNoteId, request.Date, request.DueDate,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var invoice = await _post.Handle(new PostPurchaseInvoiceCommand(id, "api"), ct);
        return Ok(invoice);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
    {
        await _cancel.Handle(new CancelPurchaseInvoiceCommand(id, "api"), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}/pdf")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPdf(Guid id, CancellationToken ct = default)
    {
        var invoice = await _getById.Handle(new GetPurchaseInvoiceByIdQuery(id), ct);
        if (invoice is null) return NotFound();
        var bytes = _pdf.GeneratePurchaseInvoice(invoice);
        return File(bytes, "application/pdf", $"Factura-compra-{invoice.Number}.pdf");
    }

    public sealed record PurchaseInvoiceLineBody(Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record PurchaseInvoiceBody(
        string? SupplierInvoiceNumber, Guid SupplierId, Guid? PurchaseDeliveryNoteId,
        DateOnly Date, DateOnly DueDate, string? Notes,
        IReadOnlyList<PurchaseInvoiceLineBody> Lines);
}
