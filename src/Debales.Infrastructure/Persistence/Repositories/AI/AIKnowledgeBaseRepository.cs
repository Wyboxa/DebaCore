using Debales.Application.AIGovernance;
using Debales.Application.Common;
using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.AI;

internal sealed class AIKnowledgeBaseRepository : BaseRepository<AIKnowledgeBase>, IAIKnowledgeBaseRepository
{
    public AIKnowledgeBaseRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<AIKnowledgeBase>> GetAllActiveAsync(CancellationToken ct = default) =>
        await DbSet.Where(k => k.IsActive).OrderBy(k => k.Title).ToListAsync(ct);

    public async Task<PagedResult<AIKnowledgeBase>> SearchAsync(string? search, int page, int pageSize, CancellationToken ct = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(k => k.Title.Contains(search) || (k.Category != null && k.Category.Contains(search)));

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(k => k.Title)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<AIKnowledgeBase>(items, total, page, pageSize);
    }
}
