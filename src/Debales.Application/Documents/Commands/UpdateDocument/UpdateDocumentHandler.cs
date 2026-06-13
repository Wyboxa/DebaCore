using Debales.Application.Common;
using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Commands.UpdateDocument;

public sealed class UpdateDocumentHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IDocumentTypeRepository _types;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentHandler(
        IDocumentRepository documents,
        IDocumentTypeRepository types,
        IUnitOfWork unitOfWork)
    {
        _documents = documents;
        _types = types;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(UpdateDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Documento '{command.Id}' no encontrado.");

        var docType = await _types.GetByIdAsync(command.DocumentTypeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Tipo de documento '{command.DocumentTypeId}' no encontrado.");

        document.Update(
            command.Title, command.Description, command.DocumentTypeId,
            command.CustomerId, command.SupplierId,
            command.FileName, command.FileSizeBytes, command.MimeType,
            command.Notes, command.DocumentDate, command.UpdatedBy);

        _documents.Update(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreateDocumentHandler.ToDto(document, docType.Name, null, null);
    }
}
