using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetCustomerStatement;

public sealed record GetCustomerStatementQuery(
    Guid CustomerId,
    DateOnly? From = null,
    DateOnly? To = null);

public sealed class GetCustomerStatementHandler(
    ICustomerRepository customerRepo,
    ISalesInvoiceRepository invoiceRepo,
    ISalesCreditNoteRepository creditNoteRepo,
    ICustomerPaymentRepository paymentRepo)
{
    public async Task<StatementDto> Handle(GetCustomerStatementQuery query, CancellationToken ct = default)
    {
        var customer = await customerRepo.GetByIdAsync(query.CustomerId, ct)
            ?? throw new InvalidOperationException($"Customer {query.CustomerId} not found.");

        var invoices = await invoiceRepo.GetByCustomerForStatementAsync(query.CustomerId, query.From, query.To, ct);
        var creditNotes = await creditNoteRepo.GetByCustomerForStatementAsync(query.CustomerId, query.From, query.To, ct);
        var payments = await paymentRepo.GetByCustomerForStatementAsync(query.CustomerId, query.From, query.To, ct);

        var lines = new List<StatementLineDto>();

        foreach (var inv in invoices.Where(i => i.Status == SalesInvoiceStatus.Posted))
            lines.Add(new StatementLineDto(inv.Date, "Factura", inv.Number, $"Factura cliente", inv.Total, 0m, 0m));

        foreach (var cn in creditNotes.Where(c => c.Status == SalesCreditNoteStatus.Posted))
            lines.Add(new StatementLineDto(cn.Date, "Rectificativa", cn.Number, "Factura rectificativa", 0m, cn.Total, 0m));

        foreach (var p in payments)
            lines.Add(new StatementLineDto(p.Date, "Cobro", p.Number, "Cobro cliente", 0m, p.Amount, 0m));

        lines = lines.OrderBy(l => l.Date).ThenBy(l => l.Reference).ToList();

        decimal running = 0m;
        var finalLines = new List<StatementLineDto>(lines.Count);
        foreach (var l in lines)
        {
            running += l.Debit - l.Credit;
            finalLines.Add(l with { Balance = running });
        }

        return new StatementDto(
            ThirdPartyId: query.CustomerId,
            ThirdPartyName: customer.Name,
            From: query.From,
            To: query.To,
            TotalDebit: finalLines.Sum(l => l.Debit),
            TotalCredit: finalLines.Sum(l => l.Credit),
            Balance: running,
            Lines: finalLines);
    }
}
