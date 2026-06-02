using Debales.Application.CRM.Activities.DTOs;
using Debales.Application.CRM.Contacts.DTOs;
using Debales.Application.CRM.Customers.DTOs;
using Debales.Application.CRM.Notes.DTOs;
using Debales.Application.CRM.Opportunities.DTOs;

namespace Debales.Application.AI.ERP;

public sealed record CustomerERPContext(
    CustomerDetailDto Customer,
    IReadOnlyList<ContactDto> Contacts,
    IReadOnlyList<ActivityDto> RecentActivities,
    IReadOnlyList<NoteDto> RecentNotes,
    IReadOnlyList<OpportunityDto> Opportunities,
    IReadOnlyList<InvoiceSummaryAI> RecentInvoices,
    decimal TotalBilled,
    IReadOnlyList<ReceivableSummaryAI> PendingReceivables,
    decimal TotalPending);
