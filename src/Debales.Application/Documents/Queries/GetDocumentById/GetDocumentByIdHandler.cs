using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Queries.GetDocumentById;

public sealed class GetDocumentByIdHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IDocumentTypeRepository _types;

    public GetDocumentByIdHandler(IDocumentRepository documents, IDocumentTypeRepository types)
    {
        _documents = documents;
        _types = types;
    }

    public async Task<DocumentDto?> Handle(GetDocumentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetByIdAsync(query.Id, cancellationToken);
        if (document is null) return null;

        var docType = await _types.GetByIdAsync(document.DocumentTypeId, cancellationToken);
        return CreateDocumentHandler.ToDto(document, docType?.Name ?? "—", null, null);
    }
}
