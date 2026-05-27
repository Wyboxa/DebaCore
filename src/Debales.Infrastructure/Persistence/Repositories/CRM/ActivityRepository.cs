using Debales.Application.CRM.Activities;
using Debales.Domain.CRM.Activities;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.CRM;

internal sealed class ActivityRepository : BaseRepository<Activity>, IActivityRepository
{
    public ActivityRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Activity>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync(cancellationToken);
}
