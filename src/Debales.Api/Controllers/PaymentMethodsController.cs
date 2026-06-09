using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.Commands.UpdatePaymentMethod;
using Debales.Application.Sales.Commands.DeletePaymentMethod;
using Debales.Application.Sales.Queries.GetPaymentMethods;
using Debales.Application.Sales.Queries.GetPaymentMethodById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PaymentMethodsController : ControllerBase
{
    private readonly GetPaymentMethodsHandler _getAll;
    private readonly GetPaymentMethodByIdHandler _getById;
    private readonly CreatePaymentMethodHandler _create;
    private readonly UpdatePaymentMethodHandler _update;
    private readonly DeletePaymentMethodHandler _delete;

    public PaymentMethodsController(
        GetPaymentMethodsHandler getAll, GetPaymentMethodByIdHandler getById,
        CreatePaymentMethodHandler create, UpdatePaymentMethodHandler update,
        DeletePaymentMethodHandler delete)
    {
        _getAll = getAll; _getById = getById; _create = create; _update = update; _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] bool? isActive,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetPaymentMethodsQuery(search, isActive, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetPaymentMethodByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentMethodRequest request)
    {
        var result = await _create.Handle(new CreatePaymentMethodCommand(
            request.Name, request.Code, request.Description, User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentMethodRequest request)
    {
        var result = await _update.Handle(new UpdatePaymentMethodCommand(
            id, request.Name, request.Code, request.Description, request.IsActive, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.Handle(new DeletePaymentMethodCommand(id, User.Identity?.Name ?? "api"));
        return NoContent();
    }
}

public sealed record CreatePaymentMethodRequest(string Name, string? Code, string? Description);
public sealed record UpdatePaymentMethodRequest(string Name, string? Code, string? Description, bool IsActive);
