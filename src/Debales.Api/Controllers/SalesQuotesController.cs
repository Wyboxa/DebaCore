using Debales.Application.Sales.Commands.AcceptSalesQuote;
using Debales.Application.Sales.Commands.ConvertQuoteToOrder;
using Debales.Application.Sales.Commands.CreateSalesQuote;
using Debales.Application.Sales.Commands.RejectSalesQuote;
using Debales.Application.Sales.Commands.SendSalesQuote;
using Debales.Application.Sales.Queries.GetSalesQuoteById;
using Debales.Application.Sales.Queries.GetSalesQuotes;
using Debales.Domain.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales/quotes")]
[Authorize]
[RequiresModule("Sales")]
public sealed class SalesQuotesController : ControllerBase
{
    private readonly CreateSalesQuoteHandler _create;
    private readonly SendSalesQuoteHandler _send;
    private readonly AcceptSalesQuoteHandler _accept;
    private readonly RejectSalesQuoteHandler _reject;
    private readonly ConvertQuoteToOrderHandler _convert;
    private readonly GetSalesQuotesHandler _getAll;
    private readonly GetSalesQuoteByIdHandler _getById;

    public SalesQuotesController(
        CreateSalesQuoteHandler create,
        SendSalesQuoteHandler send,
        AcceptSalesQuoteHandler accept,
        RejectSalesQuoteHandler reject,
        ConvertQuoteToOrderHandler convert,
        GetSalesQuotesHandler getAll,
        GetSalesQuoteByIdHandler getById)
    {
        _create = create;
        _send = send;
        _accept = accept;
        _reject = reject;
        _convert = convert;
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? customerId,
        [FromQuery] SalesQuoteStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(new GetSalesQuotesQuery(search, customerId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var quote = await _getById.Handle(new GetSalesQuoteByIdQuery(id), ct);
        return quote is null ? NotFound() : Ok(quote);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuoteBody request, CancellationToken ct = default)
    {
        var lines = request.Lines
            .Select(l => new CreateSalesQuoteLineRequest(l.ItemId, l.Description, l.Quantity, l.UnitPrice, l.TaxRate))
            .ToList();

        var quote = await _create.Handle(
            new CreateSalesQuoteCommand(
                request.CustomerId, request.Date, request.ValidUntil,
                request.Notes, lines, "api"), ct);

        return CreatedAtAction(nameof(GetById), new { id = quote.Id }, quote);
    }

    [HttpPost("{id:guid}/send")]
    public async Task<IActionResult> Send(Guid id, CancellationToken ct = default)
    {
        await _send.Handle(new SendSalesQuoteCommand(id, "api"), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<IActionResult> Accept(Guid id, CancellationToken ct = default)
    {
        await _accept.Handle(new AcceptSalesQuoteCommand(id, "api"), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken ct = default)
    {
        await _reject.Handle(new RejectSalesQuoteCommand(id, "api"), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/convert-to-order")]
    public async Task<IActionResult> ConvertToOrder(Guid id, [FromBody] ConvertQuoteBody request, CancellationToken ct = default)
    {
        var order = await _convert.Handle(
            new ConvertQuoteToOrderCommand(id, request.RequestedDeliveryDate, "api"), ct);
        return Ok(order);
    }
}

public sealed record CreateQuoteBody(
    Guid CustomerId,
    DateOnly Date,
    DateOnly ValidUntil,
    string? Notes,
    IReadOnlyList<QuoteLineBody> Lines);

public sealed record QuoteLineBody(
    Guid ItemId,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate);

public sealed record ConvertQuoteBody(DateOnly? RequestedDeliveryDate);
