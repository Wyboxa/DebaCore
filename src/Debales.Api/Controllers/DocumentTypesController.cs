using Debales.Application.Documents.Commands.CreateDocumentType;
using Debales.Application.Documents.Commands.UpdateDocumentType;
using Debales.Application.Documents.Queries.GetDocumentTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DocumentTypesController : ControllerBase
{
    private readonly CreateDocumentTypeHandler _create;
    private readonly UpdateDocumentTypeHandler _update;
    private readonly GetDocumentTypesHandler _getAll;

    public DocumentTypesController(
        CreateDocumentTypeHandler create,
        UpdateDocumentTypeHandler update,
        GetDocumentTypesHandler getAll)
    {
        _create = create;
        _update = update;
        _getAll = getAll;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _getAll.Handle(ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DocumentTypeRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _create.Handle(
                new CreateDocumentTypeCommand(request.Name, request.Description, "api"), ct);
            return Ok(result);
        }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DocumentTypeRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _update.Handle(
                new UpdateDocumentTypeCommand(id, request.Name, request.Description, "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    public sealed record DocumentTypeRequest(string Name, string? Description);
}
