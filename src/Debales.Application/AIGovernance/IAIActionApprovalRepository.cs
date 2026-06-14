using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance;

public interface IAIActionApprovalRepository : IRepository<AIActionApproval>
{
    Task<IReadOnlyList<AIActionApproval>> GetByProposalIdAsync(Guid proposalId, CancellationToken ct = default);
}
