using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance;

public interface IAIKnowledgeBaseRepository : IRepository<AIKnowledgeBase>
{
    Task<IReadOnlyList<AIKnowledgeBase>> GetAllActiveAsync(CancellationToken ct = default);
    Task<PagedResult<AIKnowledgeBase>> SearchAsync(string? search, int page, int pageSize, CancellationToken ct = default);
}
