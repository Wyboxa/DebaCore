using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesQuoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesQuote;

public sealed class CreateSalesQuoteHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreateSalesQuoteHandler(ISalesQuoteRepository quotes, IItemRepository items, IUnitOfWork uow)
    {
        _quotes = quotes;
        _items = items;
        _uow = uow;
    }

    public async Task<SalesQuoteDetailDto> Handle(CreateSalesQuoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("A quote must have at least one line.");

        var number = await _quotes.GetNextNumberAsync(cancellationToken);

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
