using Debales.Application.AIGovernance;
using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.AI;

internal sealed class AIActionApprovalRepository : BaseRepository<AIActionApproval>, IAIActionApprovalRepository
{
    public AIActionApprovalRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<AIActionApproval>> GetByProposalIdAsync(Guid proposalId, CancellationToken ct = default) =>
        await DbSet.Where(a => a.AIActionProposalId == proposalId).OrderBy(a => a.ReviewedAt).ToListAsync(ct);
}
