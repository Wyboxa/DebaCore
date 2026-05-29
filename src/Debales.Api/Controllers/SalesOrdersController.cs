using Debales.Application.Sales.Commands.CancelSalesOrder;
using Debales.Application.Sales.Commands.ConfirmSalesOrder;
using Debales.Application.Sales.Commands.CreateSalesOrder;
using Debales.Application.Sales.Queries.GetSalesOrderById;
using Debales.Application.Sales.Queries.GetSalesOrders;
using Debales.Domain.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales/orders")]
[Authorize]
public sealed class SalesOrdersController : ControllerBase
{
    private readonly CreateSalesOrderHandler _create;
    private readonly ConfirmSalesOrderHandler _confirm;
    private readonly CancelSalesOrderHandler _cancel;
    private readonly GetSalesOrdersHandler _getAll;
    private readonly GetSalesOrderByIdHandler _getById;

    public SalesOrdersController(
        CreateSalesOrderHandler create,
        ConfirmSalesOrderHandler confirm,
        CancelSalesOrderHandler cancel,
        GetSalesOrdersHandler getAll,
        GetSalesOrderByIdHandler getById)
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
        [FromQuery] Guid? customerId,
        [FromQuery] SalesOrderStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSalesOrdersQuery(search, customerId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var order = await _getById.Handle(new GetSalesOrderByIdQuery(id), ct);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesOrderBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreateSalesOrderLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var order = await _create.Handle(
            new CreateSalesOrderCommand(
                request.CustomerId, request.Date, request.RequestedDeliveryDate,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken ct = default)
    {
        var order = await _confirm.Handle(new ConfirmSalesOrderCommand(id, "api"), ct);
        return Ok(order);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
    {
        await _cancel.Handle(new CancelSalesOrderCommand(id, "api"), ct);
        return NoContent();
    }

    public sealed record SalesOrderLineBody(
        Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

    public sealed record SalesOrderBody(
        Guid CustomerId, DateOnly Date, DateOnly? RequestedDeliveryDate,
        string? Notes, IReadOnlyList<SalesOrderLineBody> Lines);
}
