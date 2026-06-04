using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesQuoteById;

public sealed class GetSalesQuoteByIdHandler
{
    private readonly ISalesQuoteRepository _quotes;

    public GetSalesQuoteByIdHandler(ISalesQuoteRepository quotes) => _quotes = quotes;

    public async Task<SalesQuoteDetailDto?> Handle(GetSalesQuoteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var quote = await _quotes.GetByIdAsync(query.Id, cancellationToken);
        return quote is null ? null : ToDto(quote);
    }

    internal static SalesQuoteDetailDto ToDto(SalesQuote q) => new(
        q.Id,
        q.Number,
        q.CustomerId,
        q.Customer?.Name ?? string.Empty,
        q.Date,
        q.ValidUntil,
        q.Status,
        StatusLabel(q.Status),
        q.Notes,
        q.Lines.Select(ToLineDto).ToList(),
        q.Subtotal,
        q.TaxAmount,
        q.Total,
        q.ConvertedToOrderId,
        q.CreatedAt,
        q.CreatedBy,
        q.UpdatedAt,
        q.UpdatedBy);

    private static SalesQuoteLineDto ToLineDto(SalesQuoteLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(SalesQuoteStatus s) => s switch
    {
        SalesQuoteStatus.Draft => "Borrador",
        SalesQuoteStatus.Sent => "Enviado",
        SalesQuoteStatus.Accepted => "Aceptado",
        SalesQuoteStatus.Rejected => "Rechazado",
        SalesQuoteStatus.Expired => "Expirado",
        _ => s.ToString()
    };
}
