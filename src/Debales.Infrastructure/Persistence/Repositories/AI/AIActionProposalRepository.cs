using Debales.Application.AIGovernance;
using Debales.Application.Common;
using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.AI;

internal sealed class AIActionProposalRepository : BaseRepository<AIActionProposal>, IAIActionProposalRepository
{
    public AIActionProposalRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<AIActionProposal>> SearchAsync(string? status, int page, int pageSize, CancellationToken ct = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<AIProposalStatus>(status, true, out var parsedStatus))
            query = query.Where(p => p.Status == parsedStatus);

        var total = await query.CountAsync(ct);
        var items = await query.OrderByDescending(p => p.ProposedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<AIActionProposal>(items, total, page, pageSize);
    }
}
