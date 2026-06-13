using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Application.Documents.Commands.DeactivateDocument;
using Debales.Application.Documents.Commands.UpdateDocument;
using Debales.Application.Documents.Commands.UploadDocumentFile;
using Debales.Application.Documents.Queries.GetDocumentById;
using Debales.Application.Documents.Queries.GetDocuments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DocumentsController : ControllerBase
{
    private readonly CreateDocumentHandler _create;
    private readonly UpdateDocumentHandler _update;
    private readonly DeactivateDocumentHandler _deactivate;
    private readonly UploadDocumentFileHandler _upload;
    private readonly GetDocumentsHandler _getAll;
    private readonly GetDocumentByIdHandler _getById;

    public DocumentsController(
        CreateDocumentHandler create,
        UpdateDocumentHandler update,
        DeactivateDocumentHandler deactivate,
        UploadDocumentFileHandler upload,
        GetDocumentsHandler getAll,
        GetDocumentByIdHandler getById)
    {
        _create = create;
        _update = update;
        _deactivate = deactivate;
        _upload = upload;
        _getAll = getAll;
        _getById = getById;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? documentTypeId,
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? supplierId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getAll.Handle(
            new GetDocumentsQuery(search, documentTypeId, customerId, supplierId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var doc = await _getById.Handle(new GetDocumentByIdQuery(id), ct);
        return doc is null ? NotFound() : Ok(doc);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _create.Handle(
                new CreateDocumentCommand(
                    request.Title, request.Description, request.DocumentTypeId,
                    request.CustomerId, request.SupplierId,
                    request.FileName, request.FileSizeBytes, request.MimeType,
                    request.Notes, request.DocumentDate, "api"), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDocumentRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _update.Handle(
                new UpdateDocumentCommand(
                    id, request.Title, request.Description, request.DocumentTypeId,
                    request.CustomerId, request.SupplierId,
                    request.FileName, request.FileSizeBytes, request.MimeType,
                    request.Notes, request.DocumentDate, "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        try
        {
            await _deactivate.Handle(new DeactivateDocumentCommand(id, "api"), ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost("{id:guid}/upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(Guid id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "El archivo está vacío." });
        try
        {
            var result = await _upload.Handle(
                new UploadDocumentFileCommand(id, file.FileName, file.Length, file.ContentType,
                    file.OpenReadStream(), User.Identity?.Name ?? "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    public sealed record CreateDocumentRequest(
        string Title,
        string? Description,
        Guid DocumentTypeId,
        Guid? CustomerId,
        Guid? SupplierId,
        string? FileName,
        long? FileSizeBytes,
        string? MimeType,
        string? Notes,
        DateTime DocumentDate);

    public sealed record UpdateDocumentRequest(
        string Title,
        string? Description,
        Guid DocumentTypeId,
        Guid? CustomerId,
        Guid? SupplierId,
        string? FileName,
        long? FileSizeBytes,
        string? MimeType,
        string? Notes,
        DateTime DocumentDate);
}
