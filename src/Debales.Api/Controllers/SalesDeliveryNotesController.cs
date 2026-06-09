using Debales.Application.Sales.Commands.CreateSalesDeliveryNote;
using Debales.Application.Sales.Commands.PostSalesDeliveryNote;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Application.Sales.Queries.GetSalesDeliveryNotes;
using Debales.Domain.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales/delivery-notes")]
[Authorize]
[RequiresModule("Sales")]
public sealed class SalesDeliveryNotesController : ControllerBase
{
    private readonly CreateSalesDeliveryNoteHandler _create;
    private readonly PostSalesDeliveryNoteHandler _post;
    private readonly GetSalesDeliveryNotesHandler _getAll;
    private readonly GetSalesDeliveryNoteByIdHandler _getById;

    public SalesDeliveryNotesController(
        CreateSalesDeliveryNoteHandler create,
        PostSalesDeliveryNoteHandler post,
        GetSalesDeliveryNotesHandler getAll,
        GetSalesDeliveryNoteByIdHandler getById)
    {
        _create = create;
        _post = post;
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? customerId,
        [FromQuery] SalesDeliveryNoteStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSalesDeliveryNotesQuery(search, customerId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var note = await _getById.Handle(new GetSalesDeliveryNoteByIdQuery(id), ct);
        return note is null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesDeliveryNoteBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreateSalesDeliveryNoteLineRequest(l.SalesOrderLineId, l.SalesOrderId, l.ItemId, l.Description, l.Quantity))
            .ToList();

        var note = await _create.Handle(
            new CreateSalesDeliveryNoteCommand(
                request.CustomerId, request.SalesOrderId, request.Date,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
    }

    [HttpPost("{id:guid}/post")]
    public async Task<IActionResult> Post(Guid id, CancellationToken ct = default)
    {
        var note = await _post.Handle(new PostSalesDeliveryNoteCommand(id, "api"), ct);
        return Ok(note);
    }

    public sealed record SalesDeliveryNoteLineBody(
        Guid? SalesOrderLineId, Guid? SalesOrderId, Guid ItemId, string? Description, decimal Quantity);

    public sealed record SalesDeliveryNoteBody(
        Guid CustomerId, Guid? SalesOrderId, DateOnly Date,
        string? Notes, IReadOnlyList<SalesDeliveryNoteLineBody> Lines);
}
