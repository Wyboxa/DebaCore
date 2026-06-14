using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance;

public interface IAIRuleRepository : IRepository<AIRule>
{
    Task<IReadOnlyList<AIRule>> GetAllActiveAsync(CancellationToken ct = default);
}
