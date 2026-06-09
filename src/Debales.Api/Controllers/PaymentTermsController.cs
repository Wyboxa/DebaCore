using Debales.Application.Sales.Commands.CreatePaymentTerm;
using Debales.Application.Sales.Commands.UpdatePaymentTerm;
using Debales.Application.Sales.Commands.DeletePaymentTerm;
using Debales.Application.Sales.Queries.GetPaymentTerms;
using Debales.Application.Sales.Queries.GetPaymentTermById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PaymentTermsController : ControllerBase
{
    private readonly GetPaymentTermsHandler _getAll;
    private readonly GetPaymentTermByIdHandler _getById;
    private readonly CreatePaymentTermHandler _create;
    private readonly UpdatePaymentTermHandler _update;
    private readonly DeletePaymentTermHandler _delete;

    public PaymentTermsController(
        GetPaymentTermsHandler getAll,
        GetPaymentTermByIdHandler getById,
        CreatePaymentTermHandler create,
        UpdatePaymentTermHandler update,
        DeletePaymentTermHandler delete)
    {
        _getAll = getAll;
        _getById = getById;
        _create = create;
        _update = update;
        _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] bool? isActive,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetPaymentTermsQuery(search, isActive, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetPaymentTermByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentTermRequest request)
    {
        var lines = request.Lines.Select(l => new CreatePaymentTermLineRequest(l.DueDays, l.Percentage)).ToList();
        var result = await _create.Handle(new CreatePaymentTermCommand(request.Name, request.Description, lines, User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentTermRequest request)
    {
        var lines = request.Lines.Select(l => new UpdatePaymentTermLineRequest(l.DueDays, l.Percentage)).ToList();
        var result = await _update.Handle(new UpdatePaymentTermCommand(id, request.Name, request.Description, request.IsActive, lines, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.Handle(new DeletePaymentTermCommand(id, User.Identity?.Name ?? "api"));
        return NoContent();
    }
}

public sealed record PaymentTermLineRequest(int DueDays, decimal Percentage);
public sealed record CreatePaymentTermRequest(string Name, string? Description, List<PaymentTermLineRequest> Lines);
public sealed record UpdatePaymentTermRequest(string Name, string? Description, bool IsActive, List<PaymentTermLineRequest> Lines);
