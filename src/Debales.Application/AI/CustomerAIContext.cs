using Debales.Application.CRM.Activities.DTOs;
using Debales.Application.CRM.Contacts.DTOs;
using Debales.Application.CRM.Customers.DTOs;
using Debales.Application.CRM.Notes.DTOs;
using Debales.Application.CRM.Opportunities.DTOs;

namespace Debales.Application.AI;

public sealed record CustomerAIContext(
    CustomerDetailDto Customer,
    IReadOnlyList<ContactDto> Contacts,
    IReadOnlyList<ActivityDto> RecentActivities,
    IReadOnlyList<NoteDto> RecentNotes,
    IReadOnlyList<OpportunityDto> Opportunities);
