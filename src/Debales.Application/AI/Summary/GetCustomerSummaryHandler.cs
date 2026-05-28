using Debales.Application.AI.Chat;
using Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;
using Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;
using Debales.Application.CRM.Customers.Queries.GetCustomerById;
using Debales.Application.CRM.Notes.Queries.GetNotesByCustomer;
using Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;

namespace Debales.Application.AI.Summary;

public sealed class GetCustomerSummaryHandler
{
    private readonly IAIService _ai;
    private readonly GetCustomerByIdHandler _getCustomer;
    private readonly GetContactsByCustomerHandler _getContacts;
    private readonly GetActivitiesByCustomerHandler _getActivities;
    private readonly GetNotesByCustomerHandler _getNotes;
    private readonly GetOpportunitiesByCustomerHandler _getOpportunities;

    public GetCustomerSummaryHandler(
        IAIService ai,
        GetCustomerByIdHandler getCustomer,
        GetContactsByCustomerHandler getContacts,
        GetActivitiesByCustomerHandler getActivities,
        GetNotesByCustomerHandler getNotes,
        GetOpportunitiesByCustomerHandler getOpportunities)
    {
        _ai = ai;
        _getCustomer = getCustomer;
        _getContacts = getContacts;
        _getActivities = getActivities;
        _getNotes = getNotes;
        _getOpportunities = getOpportunities;
    }

    public async Task<string> Handle(GetCustomerSummaryQuery query, CancellationToken ct = default)
    {
        var customer = await _getCustomer.Handle(new GetCustomerByIdQuery(query.CustomerId), ct)
            ?? throw new KeyNotFoundException($"Cliente '{query.CustomerId}' no encontrado.");

        var contacts = await _getContacts.Handle(new GetContactsByCustomerQuery(query.CustomerId), ct);
        var activities = await _getActivities.Handle(new GetActivitiesByCustomerQuery(query.CustomerId), ct);
        var notes = await _getNotes.Handle(new GetNotesByCustomerQuery(query.CustomerId), ct);
        var opportunities = await _getOpportunities.Handle(new GetOpportunitiesByCustomerQuery(query.CustomerId), ct);

        var context = new CustomerAIContext(customer, contacts, activities, notes, opportunities);
        return await _ai.GetCustomerSummaryAsync(context, ct);
    }
}
