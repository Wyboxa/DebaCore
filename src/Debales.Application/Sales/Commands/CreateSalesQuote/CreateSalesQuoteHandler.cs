using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesQuoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesQuote;

public sealed class CreateSalesQuoteHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreateSalesQuoteHandler(ISalesQuoteRepository quotes, IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _quotes = quotes;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesQuoteDetailDto> Handle(CreateSalesQuoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("A quote must have at least one line.");

        var serie = await _series.GetByCodeAsync("PRE", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'PRE' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var quote = SalesQuote.Create(
            number,
            command.CustomerId,
            command.Date,
            command.ValidUntil,
            command.Notes,
            command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            quote.AddLine(
                item.Id, item.Code, item.Name,
                lineReq.Description,
                lineReq.Quantity,
                lineReq.UnitPrice,
                lineReq.TaxRate);
        }

        await _quotes.AddAsync(quote, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _quotes.GetByIdAsync(quote.Id, cancellationToken);
        return GetSalesQuoteByIdHandler.ToDto(saved!);
    }
}
