using Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;
using Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;
using Debales.Application.CRM.Customers.Queries.GetCustomerById;
using Debales.Application.CRM.Notes.Queries.GetNotesByCustomer;
using Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;
using Debales.Application.Sales;
using Debales.Domain.Sales;

namespace Debales.Application.AI.ERP;

public sealed record GetCustomerERPSummaryQuery(Guid CustomerId);

public sealed class GetCustomerERPSummaryHandler
{
    private readonly IAIService _ai;
    private readonly GetCustomerByIdHandler _getCustomer;
    private readonly GetContactsByCustomerHandler _getContacts;
    private readonly GetActivitiesByCustomerHandler _getActivities;
    private readonly GetNotesByCustomerHandler _getNotes;
    private readonly GetOpportunitiesByCustomerHandler _getOpportunities;
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IReceivableRepository _receivables;

    public GetCustomerERPSummaryHandler(
        IAIService ai,
        GetCustomerByIdHandler getCustomer,
        GetContactsByCustomerHandler getContacts,
        GetActivitiesByCustomerHandler getActivities,
        GetNotesByCustomerHandler getNotes,
        GetOpportunitiesByCustomerHandler getOpportunities,
        ISalesInvoiceRepository invoices,
        IReceivableRepository receivables)
    {
        _ai = ai;
        _getCustomer = getCustomer;
        _getContacts = getContacts;
        _getActivities = getActivities;
        _getNotes = getNotes;
        _getOpportunities = getOpportunities;
        _invoices = invoices;
        _receivables = receivables;
    }

    public async Task<string> Handle(GetCustomerERPSummaryQuery query, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var customer      = await _getCustomer.Handle(new GetCustomerByIdQuery(query.CustomerId), ct)
            ?? throw new KeyNotFoundException($"Cliente '{query.CustomerId}' no encontrado.");
        var contacts      = await _getContacts.Handle(new GetContactsByCustomerQuery(query.CustomerId), ct);
        var activities    = await _getActivities.Handle(new GetActivitiesByCustomerQuery(query.CustomerId), ct);
        var notes         = await _getNotes.Handle(new GetNotesByCustomerQuery(query.CustomerId), ct);
        var opportunities = await _getOpportunities.Handle(new GetOpportunitiesByCustomerQuery(query.CustomerId), ct);
        var invoiceResult = await _invoices.SearchAsync(null, query.CustomerId, null, 1, 20, ct);
        var receivResult  = await _receivables.SearchAsync(null, query.CustomerId, ReceivableStatus.Pending, 1, 50, ct);

        var invoices = invoiceResult.Items
            .Select(i => new InvoiceSummaryAI(i.Number, customer.Name, i.Date, i.DueDate, i.Total,
                i.Status == SalesInvoiceStatus.Posted ? "Contabilizada" : "Borrador"))
            .ToList();

        var receivables = receivResult.Items
            .Select(r => new ReceivableSummaryAI(r.Number, customer.Name, r.DueDate, r.OriginalAmount, "Pendiente", r.DueDate < today))
            .ToList();

        var context = new CustomerERPContext(
            customer, contacts, activities, notes, opportunities,
            invoices, invoices.Sum(i => i.Total),
            receivables, receivables.Sum(r => r.Amount));

        return await _ai.GetCustomerERPSummaryAsync(context, ct);
    }
}
