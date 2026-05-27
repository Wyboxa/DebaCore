using Debales.Application.CRM.Opportunities;
using Debales.Domain.CRM.Opportunities;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.CRM;

internal sealed class OpportunityRepository : BaseRepository<Opportunity>, IOpportunityRepository
{
    public OpportunityRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Opportunity>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
}
