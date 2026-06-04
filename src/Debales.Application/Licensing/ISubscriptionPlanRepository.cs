using Debales.Application.Common;
using Debales.Domain.Licensing;

namespace Debales.Application.Licensing;

public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    Task<SubscriptionPlan?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<List<SubscriptionPlan>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
