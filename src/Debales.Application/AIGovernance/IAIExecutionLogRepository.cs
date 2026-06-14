using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance;

public interface IAIExecutionLogRepository : IRepository<AIExecutionLog>
{
    Task<PagedResult<AIExecutionLog>> SearchAsync(int page, int pageSize, CancellationToken ct = default);
}
