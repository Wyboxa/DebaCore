using Debales.Application.Common;
using Debales.Application.Suppliers;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetSupplierStatement;

public sealed record GetSupplierStatementQuery(
    Guid SupplierId,
    DateOnly? From = null,
    DateOnly? To = null);

public sealed class GetSupplierStatementHandler(
    ISupplierRepository supplierRepo,
    IPurchaseInvoiceRepository invoiceRepo,
    IPurchaseCreditNoteRepository creditNoteRepo,
    ISupplierPaymentRepository paymentRepo)
{
    public async Task<StatementDto> Handle(GetSupplierStatementQuery query, CancellationToken ct = default)
    {
        var supplier = await supplierRepo.GetByIdAsync(query.SupplierId, ct)
            ?? throw new InvalidOperationException($"Supplier {query.SupplierId} not found.");

        var invoices = await invoiceRepo.GetBySupplierForStatementAsync(query.SupplierId, query.From, query.To, ct);
        var creditNotes = await creditNoteRepo.GetBySupplierForStatementAsync(query.SupplierId, query.From, query.To, ct);
        var payments = await paymentRepo.GetBySupplierForStatementAsync(query.SupplierId, query.From, query.To, ct);

        var lines = new List<StatementLineDto>();

        foreach (var inv in invoices.Where(i => i.Status == PurchaseInvoiceStatus.Posted))
            lines.Add(new StatementLineDto(inv.Date, "Factura", inv.Number, "Factura proveedor", 0m, inv.Total, 0m));

        foreach (var cn in creditNotes.Where(c => c.Status == PurchaseCreditNoteStatus.Posted))
            lines.Add(new StatementLineDto(cn.Date, "Rectificativa", cn.Number, "Factura rectificativa", cn.Total, 0m, 0m));

        foreach (var p in payments)
            lines.Add(new StatementLineDto(p.Date, "Pago", p.Number, "Pago proveedor", p.Amount, 0m, 0m));

        lines = lines.OrderBy(l => l.Date).ThenBy(l => l.Reference).ToList();

        // Balance = crédito - débito (lo que debemos al proveedor)
        decimal running = 0m;
        var finalLines = new List<StatementLineDto>(lines.Count);
        foreach (var l in lines)
        {
            running += l.Credit - l.Debit;
            finalLines.Add(l with { Balance = running });
        }

        return new StatementDto(
            ThirdPartyId: query.SupplierId,
            ThirdPartyName: supplier.Name,
            From: query.From,
            To: query.To,
            TotalDebit: finalLines.Sum(l => l.Debit),
            TotalCredit: finalLines.Sum(l => l.Credit),
            Balance: running,
            Lines: finalLines);
    }
}
