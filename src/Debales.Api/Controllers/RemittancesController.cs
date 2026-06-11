using Debales.Application.Accounting.Commands.AddRemittanceLine;
using Debales.Application.Accounting.Commands.ConfirmRemittance;
using Debales.Application.Accounting.Commands.CreateRemittance;
using Debales.Application.Accounting.Commands.DeleteRemittance;
using Debales.Application.Accounting.Commands.FailRemittance;
using Debales.Application.Accounting.Commands.RemoveRemittanceLine;
using Debales.Application.Accounting.Commands.SendRemittance;
using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.Queries.GetRemittanceById;
using Debales.Application.Accounting.Queries.GetRemittances;
using Debales.Domain.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class RemittancesController : ControllerBase
{
    private readonly GetRemittancesHandler _getAll;
    private readonly GetRemittanceByIdHandler _getById;
    private readonly CreateRemittanceHandler _create;
    private readonly UpdateRemittanceHandler _update;
    private readonly DeleteRemittanceHandler _delete;
    private readonly AddRemittanceLineHandler _addLine;
    private readonly RemoveRemittanceLineHandler _removeLine;
    private readonly SendRemittanceHandler _send;
    private readonly ConfirmRemittanceHandler _confirm;
    private readonly FailRemittanceHandler _fail;

    public RemittancesController(
        GetRemittancesHandler getAll, GetRemittanceByIdHandler getById,
        CreateRemittanceHandler create, UpdateRemittanceHandler update, DeleteRemittanceHandler delete,
        AddRemittanceLineHandler addLine, RemoveRemittanceLineHandler removeLine,
        SendRemittanceHandler send, ConfirmRemittanceHandler confirm, FailRemittanceHandler fail)
    {
        _getAll = getAll; _getById = getById; _create = create; _update = update; _delete = delete;
        _addLine = addLine; _removeLine = removeLine; _send = send; _confirm = confirm; _fail = fail;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] RemittanceType? type, [FromQuery] RemittanceStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _getAll.Handle(new GetRemittancesQuery(type, status, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getById.Handle(new GetRemittanceByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRemittanceRequest request)
    {
        var result = await _create.Handle(new CreateRemittanceCommand(
            request.Date, request.BankAccountId, request.Type, request.Notes,
            User.Identity?.Name ?? "api"));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRemittanceRequest request)
        => Ok(await _update.Handle(new UpdateRemittanceCommand(id, request.Notes, User.Identity?.Name ?? "api")));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _delete.Handle(new DeleteRemittanceCommand(id, User.Identity?.Name ?? "api"));
        return NoContent();
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid id, [FromBody] AddRemittanceLineRequest request)
        => Ok(await _addLine.Handle(new AddRemittanceLineCommand(id, request.DocumentId, request.Amount, User.Identity?.Name ?? "api")));

    [HttpDelete("{id:guid}/lines/{documentId:guid}")]
    public async Task<IActionResult> RemoveLine(Guid id, Guid documentId)
        => Ok(await _removeLine.Handle(new RemoveRemittanceLineCommand(id, documentId, User.Identity?.Name ?? "api")));

    [HttpPost("{id:guid}/send")]
    public async Task<IActionResult> Send(Guid id)
        => Ok(await _send.Handle(new SendRemittanceCommand(id, User.Identity?.Name ?? "api")));

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
        => Ok(await _confirm.Handle(new ConfirmRemittanceCommand(id, User.Identity?.Name ?? "api")));

    [HttpPost("{id:guid}/fail")]
    public async Task<IActionResult> Fail(Guid id, [FromBody] FailRemittanceRequest request)
        => Ok(await _fail.Handle(new FailRemittanceCommand(id, request.Reason, User.Identity?.Name ?? "api")));
}

public sealed record CreateRemittanceRequest(DateOnly Date, Guid BankAccountId, RemittanceType Type, string? Notes);
public sealed record UpdateRemittanceRequest(string? Notes);
public sealed record AddRemittanceLineRequest(Guid DocumentId, decimal Amount);
public sealed record FailRemittanceRequest(string Reason);
