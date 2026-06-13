using Debales.Application.Common;
using Debales.Domain.Documents;

namespace Debales.Application.Documents;

public interface IDocumentRepository : IRepository<Document>
{
    Task<PagedResult<Document>> SearchAsync(
        string? search,
        Guid? documentTypeId,
        Guid? customerId,
        Guid? supplierId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
