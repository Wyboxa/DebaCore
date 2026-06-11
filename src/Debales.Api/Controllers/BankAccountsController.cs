using Debales.Application.Accounting.Commands.CreateBankAccount;
using Debales.Application.Accounting.Commands.UpdateBankAccount;
using Debales.Application.Accounting.Commands.DeleteBankAccount;
using Debales.Application.Accounting.Queries.GetBankAccounts;
using Debales.Application.Accounting.Queries.GetBankAccountById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BankAccountsController : ControllerBase
{
    private readonly GetBankAccountsHandler _getAll;
    private readonly GetBankAccountByIdHandler _getById;
    private readonly CreateBankAccountHandler _create;
    private readonly UpdateBankAccountHandler _update;
    private readonly DeleteBankAccountHandler _delete;

    public BankAccountsController(
        GetBankAccountsHandler getAll, GetBankAccountByIdHandler getById,
        CreateBankAccountHandler create, UpdateBankAccountHandler update,
        DeleteBankAccountHandler delete)
    {
        _getAll = getAll; _getById = getById; _create = create; _update = update; _delete = delete;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] bool? isActive,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetBankAccountsQuery(search, isActive, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetBankAccountByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountRequest request)
    {
        var result = await _create.Handle(new CreateBankAccountCommand(
            request.Name, request.BankName, request.Iban, request.Bic,
            request.CurrencyCode, request.AccountId, User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankAccountRequest request)
    {
        var result = await _update.Handle(new UpdateBankAccountCommand(
            id, request.Name, request.BankName, request.Iban, request.Bic,
            request.CurrencyCode, request.AccountId, request.IsActive, User.Identity?.Name ?? "api"));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.Handle(new DeleteBankAccountCommand(id, User.Identity?.Name ?? "api"));
        return NoContent();
    }
}

public sealed record CreateBankAccountRequest(
    string Name, string? BankName, string? Iban, string? Bic,
    string? CurrencyCode, Guid? AccountId);

public sealed record UpdateBankAccountRequest(
    string Name, string? BankName, string? Iban, string? Bic,
    string? CurrencyCode, Guid? AccountId, bool IsActive);
