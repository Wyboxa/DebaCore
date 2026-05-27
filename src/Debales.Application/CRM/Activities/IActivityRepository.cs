using Debales.Application.Common;
using Debales.Domain.CRM.Activities;

namespace Debales.Application.CRM.Activities;

public interface IActivityRepository : IRepository<Activity>
{
    Task<IReadOnlyList<Activity>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
