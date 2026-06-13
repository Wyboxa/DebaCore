using Debales.Application.Common;
using Debales.Domain.Documents;

namespace Debales.Application.Documents;

public interface IDocumentTypeRepository : IRepository<DocumentType>
{
    Task<IReadOnlyList<DocumentType>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
