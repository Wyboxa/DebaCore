using Debales.Application.CRM.Opportunities.Commands.CreateOpportunity;
using Debales.Application.CRM.Opportunities.DTOs;

namespace Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;

public sealed record GetOpportunitiesByCustomerQuery(Guid CustomerId);

public sealed class GetOpportunitiesByCustomerHandler
{
    private readonly IOpportunityRepository _opportunities;

    public GetOpportunitiesByCustomerHandler(IOpportunityRepository opportunities) => _opportunities = opportunities;

    public async Task<IReadOnlyList<OpportunityDto>> Handle(GetOpportunitiesByCustomerQuery query, CancellationToken cancellationToken = default)
    {
        var opportunities = await _opportunities.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
        return opportunities.Select(CreateOpportunityHandler.ToDto).ToList();
    }
}
