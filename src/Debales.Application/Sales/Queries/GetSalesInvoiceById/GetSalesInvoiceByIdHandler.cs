using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesInvoiceById;

public sealed class GetSalesInvoiceByIdHandler
{
    private readonly ISalesInvoiceRepository _invoices;

    public GetSalesInvoiceByIdHandler(ISalesInvoiceRepository invoices) => _invoices = invoices;

    public async Task<SalesInvoiceDetailDto?> Handle(GetSalesInvoiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(query.Id, cancellationToken);
        return invoice is null ? null : ToDto(invoice);
    }

    internal static SalesInvoiceDetailDto ToDto(SalesInvoice invoice) => new(
        invoice.Id,
        invoice.Number,
        invoice.CustomerId,
        invoice.Customer?.Name ?? string.Empty,
        invoice.SalesDeliveryNoteId,
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

    internal static SalesInvoiceSummaryDto ToSummaryDto(SalesInvoice invoice) => new(
        invoice.Id,
        invoice.Number,
        invoice.CustomerId,
        invoice.Customer?.Name ?? string.Empty,
        invoice.Date,
        invoice.DueDate,
        invoice.Status,
        StatusLabel(invoice.Status),
        invoice.Total);

    private static SalesInvoiceLineSummaryDto ToLineDto(SalesInvoiceLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(SalesInvoiceStatus s) => s switch
    {
        SalesInvoiceStatus.Draft => "Borrador",
        SalesInvoiceStatus.Posted => "Contabilizada",
        SalesInvoiceStatus.Cancelled => "Cancelada",
        _ => s.ToString()
    };
}
