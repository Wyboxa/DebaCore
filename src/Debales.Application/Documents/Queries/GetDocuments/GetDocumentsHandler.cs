using Debales.Application.Common;
using Debales.Application.Documents.Commands.CreateDocument;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Queries.GetDocuments;

public sealed class GetDocumentsHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IDocumentTypeRepository _types;

    public GetDocumentsHandler(IDocumentRepository documents, IDocumentTypeRepository types)
    {
        _documents = documents;
        _types = types;
    }

    public async Task<PagedResult<DocumentDto>> Handle(GetDocumentsQuery query, CancellationToken cancellationToken = default)
    {
        var paged = await _documents.SearchAsync(
            query.Search, query.DocumentTypeId, query.CustomerId, query.SupplierId,
            query.Page, query.PageSize, cancellationToken);

        var typeIds = paged.Items.Select(d => d.DocumentTypeId).Distinct().ToHashSet();
        var allTypes = await _types.GetAllAsync(cancellationToken);
        var typeMap = allTypes.Where(t => typeIds.Contains(t.Id)).ToDictionary(t => t.Id, t => t.Name);

        var items = paged.Items
            .Select(d => CreateDocumentHandler.ToDto(d, typeMap.GetValueOrDefault(d.DocumentTypeId, "—"), null, null))
            .ToList();

        return new PagedResult<DocumentDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
