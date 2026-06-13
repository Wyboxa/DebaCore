using Debales.Application.Common;
using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Commands.UploadDocumentFile;

public sealed class UploadDocumentFileHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IDocumentTypeRepository _types;
    private readonly IFileStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDocumentFileHandler(
        IDocumentRepository documents,
        IDocumentTypeRepository types,
        IFileStorageService storage,
        IUnitOfWork unitOfWork)
    {
        _documents = documents;
        _types = types;
        _storage = storage;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(UploadDocumentFileCommand command, CancellationToken ct = default)
    {
        var doc = await _documents.GetByIdAsync(command.DocumentId, ct)
            ?? throw new KeyNotFoundException($"Documento '{command.DocumentId}' no encontrado.");

        var storedName = await _storage.SaveAsync(command.FileName, command.Content, ct);
        doc.AttachFile(storedName, command.FileSizeBytes, command.MimeType, command.UpdatedBy);
        _documents.Update(doc);
        await _unitOfWork.SaveChangesAsync(ct);

        var docType = await _types.GetByIdAsync(doc.DocumentTypeId, ct);
        return CreateDocumentHandler.ToDto(doc, docType?.Name ?? "", null, null);
    }
}
