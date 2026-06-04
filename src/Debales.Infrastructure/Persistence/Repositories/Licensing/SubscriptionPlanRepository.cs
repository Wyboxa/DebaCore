using Debales.Application.Licensing;
using Debales.Domain.Licensing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Licensing;

internal sealed class SubscriptionPlanRepository : BaseRepository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(ApplicationDbContext context) : base(context) { }

    public async Task<SubscriptionPlan?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);

    public async Task<List<SubscriptionPlan>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.PriceMonthly)
            .ToListAsync(cancellationToken);
}
