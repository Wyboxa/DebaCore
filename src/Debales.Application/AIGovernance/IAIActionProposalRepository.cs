using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance;

public interface IAIActionProposalRepository : IRepository<AIActionProposal>
{
    Task<PagedResult<AIActionProposal>> SearchAsync(string? status, int page, int pageSize, CancellationToken ct = default);
}
