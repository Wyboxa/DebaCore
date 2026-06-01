using Debales.Application.Purchasing.Commands.CreatePurchaseCreditNote;
using Debales.Application.Purchasing.Commands.PostPurchaseCreditNote;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNoteById;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/purchasing/credit-notes")]
[Authorize]
public sealed class PurchaseCreditNotesController : ControllerBase
{
    private readonly CreatePurchaseCreditNoteHandler _create;
    private readonly PostPurchaseCreditNoteHandler _post;
    private readonly GetPurchaseCreditNotesHandler _getAll;
    private readonly GetPurchaseCreditNoteByIdHandler _getById;

    public PurchaseCreditNotesController(
        CreatePurchaseCreditNoteHandler create, PostPurchaseCreditNoteHandler post,
        GetPurchaseCreditNotesHandler getAll, GetPurchaseCreditNoteByIdHandler getById)
    {
        _create = create; _post = post; _getAll = getAll; _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? supplierId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetPurchaseCreditNotesQuery(search, supplierId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var note = await _getById.Handle(new GetPurchaseCreditNoteByIdQuery(id), ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseCreditNoteBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreatePurchaseCreditNoteLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var note = await _create.Handle(
            new CreatePurchaseCreditNoteCommand(
                request.SupplierId, request.OriginalInvoiceId,
                request.Date, request.Reason, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var note = await _post.Handle(new PostPurchaseCreditNoteCommand(id, "api"), ct);
        return Ok(note);
    }

    public sealed record PurchaseCreditNoteLineBody(Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record PurchaseCreditNoteBody(
        Guid SupplierId, Guid OriginalInvoiceId,
        DateOnly Date, string? Reason,
        IReadOnlyList<PurchaseCreditNoteLineBody> Lines);
}
