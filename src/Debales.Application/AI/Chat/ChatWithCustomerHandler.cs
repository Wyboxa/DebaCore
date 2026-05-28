using Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;
using Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;
using Debales.Application.CRM.Customers.Queries.GetCustomerById;
using Debales.Application.CRM.Notes.Queries.GetNotesByCustomer;
using Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;

namespace Debales.Application.AI.Chat;

public sealed class ChatWithCustomerHandler
{
    private readonly IAIService _ai;
    private readonly GetCustomerByIdHandler _getCustomer;
    private readonly GetContactsByCustomerHandler _getContacts;
    private readonly GetActivitiesByCustomerHandler _getActivities;
    private readonly GetNotesByCustomerHandler _getNotes;
    private readonly GetOpportunitiesByCustomerHandler _getOpportunities;

    public ChatWithCustomerHandler(
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

    public async Task<ChatResponseDto> Handle(ChatWithCustomerCommand command, CancellationToken ct = default)
    {
        var customer = await _getCustomer.Handle(new GetCustomerByIdQuery(command.CustomerId), ct)
            ?? throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        var contacts = await _getContacts.Handle(new GetContactsByCustomerQuery(command.CustomerId), ct);
        var activities = await _getActivities.Handle(new GetActivitiesByCustomerQuery(command.CustomerId), ct);
        var notes = await _getNotes.Handle(new GetNotesByCustomerQuery(command.CustomerId), ct);
        var opportunities = await _getOpportunities.Handle(new GetOpportunitiesByCustomerQuery(command.CustomerId), ct);

        var context = new CustomerAIContext(customer, contacts, activities, notes, opportunities);
        var reply = await _ai.ChatAsync(context, command.History, command.NewMessage, ct);

        return new ChatResponseDto(reply);
    }
}

