using Debales.Application.CRM.Activities.Commands.LogActivity;
using Debales.Application.CRM.Activities.DTOs;

namespace Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;

public sealed record GetActivitiesByCustomerQuery(Guid CustomerId);

public sealed class GetActivitiesByCustomerHandler
{
    private readonly IActivityRepository _activities;

    public GetActivitiesByCustomerHandler(IActivityRepository activities) => _activities = activities;

    public async Task<IReadOnlyList<ActivityDto>> Handle(GetActivitiesByCustomerQuery query, CancellationToken cancellationToken = default)
    {
        var activities = await _activities.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
        return activities.Select(LogActivityHandler.ToDto).ToList();
    }
}
