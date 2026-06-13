using Debales.Application.Common;
using Debales.Application.Documents.DTOs;
using Debales.Domain.Documents;

namespace Debales.Application.Documents.Commands.CreateDocument;

public sealed class CreateDocumentHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IDocumentTypeRepository _types;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentHandler(
        IDocumentRepository documents,
        IDocumentTypeRepository types,
        IUnitOfWork unitOfWork)
    {
        _documents = documents;
        _types = types;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(CreateDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var docType = await _types.GetByIdAsync(command.DocumentTypeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Tipo de documento '{command.DocumentTypeId}' no encontrado.");

        var document = Document.Create(
            command.Title, command.Description, command.DocumentTypeId,
            command.CustomerId, command.SupplierId,
            command.FileName, command.FileSizeBytes, command.MimeType,
            command.Notes, command.DocumentDate, command.CreatedBy);

        await _documents.AddAsync(document, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(document, docType.Name, null, null);
    }

    internal static DocumentDto ToDto(Document d, string documentTypeName, string? customerName, string? supplierName) =>
        new(d.Id, d.Title, d.Description, d.DocumentTypeId, documentTypeName,
            d.CustomerId, customerName, d.SupplierId, supplierName,
            d.FileName, d.FileSizeBytes, d.MimeType, d.Notes,
            d.DocumentDate, d.IsActive, d.CreatedAt);
}
