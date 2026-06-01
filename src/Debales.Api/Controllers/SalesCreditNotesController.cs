using Debales.Application.Sales.Commands.CreateSalesCreditNote;
using Debales.Application.Sales.Commands.PostSalesCreditNote;
using Debales.Application.Sales.Queries.GetSalesCreditNoteById;
using Debales.Application.Sales.Queries.GetSalesCreditNotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales/credit-notes")]
[Authorize]
public sealed class SalesCreditNotesController : ControllerBase
{
    private readonly CreateSalesCreditNoteHandler _create;
    private readonly PostSalesCreditNoteHandler _post;
    private readonly GetSalesCreditNotesHandler _getAll;
    private readonly GetSalesCreditNoteByIdHandler _getById;

    public SalesCreditNotesController(
        CreateSalesCreditNoteHandler create, PostSalesCreditNoteHandler post,
        GetSalesCreditNotesHandler getAll, GetSalesCreditNoteByIdHandler getById)
    {
        _create = create; _post = post; _getAll = getAll; _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? customerId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSalesCreditNotesQuery(search, customerId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var note = await _getById.Handle(new GetSalesCreditNoteByIdQuery(id), ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesCreditNoteBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreateSalesCreditNoteLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var note = await _create.Handle(
            new CreateSalesCreditNoteCommand(
                request.CustomerId, request.OriginalInvoiceId,
                request.Date, request.Reason, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var note = await _post.Handle(new PostSalesCreditNoteCommand(id, "api"), ct);
        return Ok(note);
    }

    public sealed record SalesCreditNoteLineBody(Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record SalesCreditNoteBody(
        Guid CustomerId, Guid OriginalInvoiceId,
        DateOnly Date, string? Reason,
        IReadOnlyList<SalesCreditNoteLineBody> Lines);
}
