using Debales.Application.Documents.Commands.CreateDocumentType;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Queries.GetDocumentTypes;

public sealed class GetDocumentTypesHandler
{
    private readonly IDocumentTypeRepository _types;

    public GetDocumentTypesHandler(IDocumentTypeRepository types) => _types = types;

    public async Task<IReadOnlyList<DocumentTypeDto>> Handle(CancellationToken cancellationToken = default)
    {
        var types = await _types.GetAllActiveAsync(cancellationToken);
        return types.Select(CreateDocumentTypeHandler.ToDto).ToList();
    }
}
