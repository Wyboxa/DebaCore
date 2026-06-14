using Debales.Application.AIGovernance;
using Debales.Application.Common;
using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.AI;

internal sealed class AIExecutionLogRepository : BaseRepository<AIExecutionLog>, IAIExecutionLogRepository
{
    public AIExecutionLogRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<AIExecutionLog>> SearchAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var total = await DbSet.CountAsync(ct);
        var items = await DbSet.OrderByDescending(l => l.ExecutedAt)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);
        return new PagedResult<AIExecutionLog>(items, total, page, pageSize);
    }
}
