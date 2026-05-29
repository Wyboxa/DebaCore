using Debales.Application.Purchasing.Commands.CreatePurchaseDeliveryNote;
using Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNotes;
using Debales.Domain.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/purchasing/delivery-notes")]
[Authorize]
public sealed class PurchaseDeliveryNotesController : ControllerBase
{
    private readonly CreatePurchaseDeliveryNoteHandler _create;
    private readonly PostPurchaseDeliveryNoteHandler _post;
    private readonly GetPurchaseDeliveryNotesHandler _getAll;
    private readonly GetPurchaseDeliveryNoteByIdHandler _getById;

    public PurchaseDeliveryNotesController(
        CreatePurchaseDeliveryNoteHandler create,
        PostPurchaseDeliveryNoteHandler post,
        GetPurchaseDeliveryNotesHandler getAll,
        GetPurchaseDeliveryNoteByIdHandler getById)
    {
        _create = create;
        _post = post;
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? supplierId,
        [FromQuery] PurchaseDeliveryNoteStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetPurchaseDeliveryNotesQuery(search, supplierId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var note = await _getById.Handle(new GetPurchaseDeliveryNoteByIdQuery(id), ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseDeliveryNoteBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreatePurchaseDeliveryNoteLineRequest(l.PurchaseOrderLineId, l.PurchaseOrderId, l.ItemId, l.Description, l.Quantity))
            .ToList();

        var note = await _create.Handle(
            new CreatePurchaseDeliveryNoteCommand(
                request.SupplierId, request.PurchaseOrderId, request.Date,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var note = await _post.Handle(new PostPurchaseDeliveryNoteCommand(id, "api"), ct);
        return Ok(note);
    }

    public sealed record PurchaseDeliveryNoteLineBody(
        Guid? PurchaseOrderLineId, Guid? PurchaseOrderId, Guid ItemId, string? Description, decimal Quantity);

    public sealed record PurchaseDeliveryNoteBody(
        Guid SupplierId, Guid? PurchaseOrderId, DateOnly Date,
        string? Notes, IReadOnlyList<PurchaseDeliveryNoteLineBody> Lines);
}
