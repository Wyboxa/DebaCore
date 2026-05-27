using Debales.Application.Common;
using Debales.Domain.CRM.Opportunities;

namespace Debales.Application.CRM.Opportunities;

public interface IOpportunityRepository : IRepository<Opportunity>
{
    Task<IReadOnlyList<Opportunity>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
