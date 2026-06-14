using Debales.Application.AIGovernance;
using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.AI;

internal sealed class AIRuleRepository : BaseRepository<AIRule>, IAIRuleRepository
{
    public AIRuleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<AIRule>> GetAllActiveAsync(CancellationToken ct = default) =>
        await DbSet.Where(r => r.IsActive).OrderBy(r => r.Name).ToListAsync(ct);
}
