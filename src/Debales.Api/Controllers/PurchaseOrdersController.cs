using Debales.Application.Purchasing.Commands.CancelPurchaseOrder;
using Debales.Application.Purchasing.Commands.ConfirmPurchaseOrder;
using Debales.Application.Purchasing.Commands.CreatePurchaseOrder;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;
using Debales.Application.Purchasing.Queries.GetPurchaseOrders;
using Debales.Domain.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/purchasing/orders")]
[Authorize]
[RequiresModule("Purchasing")]
public sealed class PurchaseOrdersController : ControllerBase
{
    private readonly CreatePurchaseOrderHandler _create;
    private readonly ConfirmPurchaseOrderHandler _confirm;
    private readonly CancelPurchaseOrderHandler _cancel;
    private readonly GetPurchaseOrdersHandler _getAll;
    private readonly GetPurchaseOrderByIdHandler _getById;

    public PurchaseOrdersController(
        CreatePurchaseOrderHandler create,
        ConfirmPurchaseOrderHandler confirm,
        CancelPurchaseOrderHandler cancel,
        GetPurchaseOrdersHandler getAll,
        GetPurchaseOrderByIdHandler getById)
    {
        _create = create;
        _confirm = confirm;
        _cancel = cancel;
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? supplierId,
        [FromQuery] PurchaseOrderStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetPurchaseOrdersQuery(search, supplierId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var order = await _getById.Handle(new GetPurchaseOrderByIdQuery(id), ct);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreatePurchaseOrderLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var order = await _create.Handle(
            new CreatePurchaseOrderCommand(
                request.SupplierId, request.Date, request.ExpectedReceiptDate,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken ct = default)
    {
        var order = await _confirm.Handle(new ConfirmPurchaseOrderCommand(id, "api"), ct);
        return Ok(order);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
    {
        await _cancel.Handle(new CancelPurchaseOrderCommand(id, "api"), ct);
        return NoContent();
    }

    public sealed record PurchaseOrderLineBody(
        Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record PurchaseOrderBody(
        Guid SupplierId, DateOnly Date, DateOnly? ExpectedReceiptDate,
        string? Notes, IReadOnlyList<PurchaseOrderLineBody> Lines);
}
