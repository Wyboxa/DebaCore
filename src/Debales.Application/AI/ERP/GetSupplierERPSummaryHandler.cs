using Debales.Application.Purchasing;
using Debales.Application.Suppliers.Queries.GetSupplierById;
using Debales.Domain.Purchasing;

namespace Debales.Application.AI.ERP;

public sealed record GetSupplierERPSummaryQuery(Guid SupplierId);


public sealed class GetSupplierERPSummaryHandler
{
    private readonly IAIService _ai;
    private readonly GetSupplierByIdHandler _getSupplier;
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IPayableRepository _payables;

    public GetSupplierERPSummaryHandler(
        IAIService ai,
        GetSupplierByIdHandler getSupplier,
        IPurchaseInvoiceRepository invoices,
        IPayableRepository payables)
    {
        _ai = ai;
        _getSupplier = getSupplier;
        _invoices = invoices;
        _payables = payables;
    }

    public async Task<string> Handle(GetSupplierERPSummaryQuery query, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var supplier = await _getSupplier.Handle(new GetSupplierByIdQuery(query.SupplierId), ct)
            ?? throw new KeyNotFoundException($"Proveedor '{query.SupplierId}' no encontrado.");

        var invoiceResult = await _invoices.SearchAsync(null, query.SupplierId, null, 1, 20, ct);
        var payableResult = await _payables.SearchAsync(null, query.SupplierId, PayableStatus.Pending, 1, 50, ct);

        var invoices = invoiceResult.Items
            .Select(i => new InvoiceSummaryAI(i.Number, supplier.Name, i.Date, i.DueDate, i.Total,
                i.Status == PurchaseInvoiceStatus.Posted ? "Contabilizada" : "Borrador"))
            .ToList();

        var payables = payableResult.Items
            .Select(p => new PayableSummaryAI(
                p.Number, supplier.Name, p.DueDate, p.OriginalAmount, "Pendiente", p.DueDate < today))
            .ToList();

        var context = new SupplierAIContext(
            supplier, invoices, invoices.Sum(i => i.Total),
            payables, payables.Sum(p => p.Amount));

        return await _ai.GetSupplierERPSummaryAsync(context, ct);
    }
}
