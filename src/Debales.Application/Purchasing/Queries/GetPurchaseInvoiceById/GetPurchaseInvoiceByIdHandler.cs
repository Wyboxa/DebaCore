using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;

public sealed class GetPurchaseInvoiceByIdHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;

    public GetPurchaseInvoiceByIdHandler(IPurchaseInvoiceRepository invoices) => _invoices = invoices;

    public async Task<PurchaseInvoiceDetailDto?> Handle(GetPurchaseInvoiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(query.Id, cancellationToken);
        return invoice is null ? null : ToDto(invoice);
    }

    internal static PurchaseInvoiceDetailDto ToDto(PurchaseInvoice invoice) => new(
        invoice.Id,
        invoice.Number,
        invoice.SupplierInvoiceNumber,
        invoice.SupplierId,
        invoice.Supplier?.Name ?? string.Empty,
        invoice.PurchaseDeliveryNoteId,
        invoice.Date,
        invoice.DueDate,
        invoice.Status,
        StatusLabel(invoice.Status),
        invoice.Notes,
        invoice.Lines.Select(ToLineDto).ToList(),
        invoice.Subtotal,
        invoice.TaxAmount,
        invoice.Total,
        invoice.CreatedAt,
        invoice.CreatedBy,
        invoice.UpdatedAt,
        invoice.UpdatedBy);

    internal static PurchaseInvoiceSummaryDto ToSummaryDto(PurchaseInvoice invoice) => new(
        invoice.Id,
        invoice.Number,
        invoice.SupplierInvoiceNumber,
        invoice.SupplierId,
        invoice.Supplier?.Name ?? string.Empty,
        invoice.Date,
        invoice.DueDate,
        invoice.Status,
        StatusLabel(invoice.Status),
        invoice.Total,
        invoice.PurchaseDeliveryNoteId);

    private static PurchaseInvoiceLineSummaryDto ToLineDto(PurchaseInvoiceLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(PurchaseInvoiceStatus s) => s switch
    {
        PurchaseInvoiceStatus.Draft => "Borrador",
        PurchaseInvoiceStatus.Posted => "Contabilizada",
        PurchaseInvoiceStatus.Cancelled => "Cancelada",
        _ => s.ToString()
    };
}
