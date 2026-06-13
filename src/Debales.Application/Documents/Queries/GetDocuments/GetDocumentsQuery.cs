namespace Debales.Application.Documents.Queries.GetDocuments;

public sealed record GetDocumentsQuery(
    string? Search,
    Guid? DocumentTypeId,
    Guid? CustomerId,
    Guid? SupplierId,
    int Page = 1,
    int PageSize = 20);
