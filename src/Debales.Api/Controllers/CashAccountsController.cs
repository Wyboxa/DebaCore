using Debales.Application.Accounting.Commands.CreateCashAccount;
using Debales.Application.Accounting.Commands.DeleteCashAccount;
using Debales.Application.Accounting.Commands.UpdateCashAccount;
using Debales.Application.Accounting.Queries.GetCashAccountById;
using Debales.Application.Accounting.Queries.GetCashAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CashAccountsController : ControllerBase
{
    private readonly GetCashAccountsHandler _getAll;
    private readonly GetCashAccountByIdHandler _getById;
    private readonly CreateCashAccountHandler _create;
    private readonly UpdateCashAccountHandler _update;
    private readonly DeleteCashAccountHandler _delete;

    public CashAccountsController(
        GetCashAccountsHandler getAll, GetCashAccountByIdHandler getById,
        CreateCashAccountHandler create, UpdateCashAccountHandler update,
        DeleteCashAccountHandler delete)
    {
        _getAll = getAll; _getById = getById; _create = create; _update = update; _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] bool? isActive,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetCashAccountsQuery(search, isActive, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetCashAccountByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCashAccountRequest request)
    {
        var result = await _create.Handle(new CreateCashAccountCommand(
            request.Code, request.Name, request.CurrencyCode, request.AccountId,
            User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCashAccountRequest request)
    {
        var result = await _update.Handle(new UpdateCashAccountCommand(
            id, request.Code, request.Name, request.CurrencyCode, request.AccountId,
            request.IsActive, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.Handle(new DeleteCashAccountCommand(id, User.Identity?.Name ?? "api"));
        return NoContent();
    }
}

public sealed record CreateCashAccountRequest(string Code, string Name, string? CurrencyCode, Guid? AccountId);
public sealed record UpdateCashAccountRequest(string Code, string Name, string? CurrencyCode, Guid? AccountId, bool IsActive);
