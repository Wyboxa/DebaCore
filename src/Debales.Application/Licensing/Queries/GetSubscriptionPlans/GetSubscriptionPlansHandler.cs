using Debales.Application.Licensing.DTOs;

namespace Debales.Application.Licensing.Queries.GetSubscriptionPlans;

public sealed class GetSubscriptionPlansHandler
{
    private readonly ISubscriptionPlanRepository _plans;

    public GetSubscriptionPlansHandler(ISubscriptionPlanRepository plans)
    {
        _plans = plans;
    }

    public async Task<List<SubscriptionPlanDto>> Handle(CancellationToken cancellationToken = default)
    {
        var plans = await _plans.GetAllActiveAsync(cancellationToken);

        return plans.Select(p => new SubscriptionPlanDto(
            p.Id, p.Code, p.Name, p.Description,
            p.MaxUsers, p.MaxModules, p.AllowsAI, p.PriceMonthly, p.IsActive))
            .ToList();
    }
}
