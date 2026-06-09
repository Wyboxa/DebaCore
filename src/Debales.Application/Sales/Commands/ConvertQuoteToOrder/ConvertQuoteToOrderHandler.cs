using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesOrderById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.ConvertQuoteToOrder;

public sealed class ConvertQuoteToOrderHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly ISalesOrderRepository _orders;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public ConvertQuoteToOrderHandler(ISalesQuoteRepository quotes, ISalesOrderRepository orders, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _quotes = quotes;
        _orders = orders;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesOrderDetailDto> Handle(ConvertQuoteToOrderCommand command, CancellationToken cancellationToken = default)
    {
        var quote = await _quotes.GetByIdAsync(command.QuoteId, cancellationToken)
            ?? throw new InvalidOperationException($"Presupuesto '{command.QuoteId}' no encontrado.");

        if (quote.Status != SalesQuoteStatus.Accepted)
            throw new InvalidOperationException("Solo los presupuestos aceptados pueden convertirse en pedido.");

        if (quote.ConvertedToOrderId.HasValue)
            throw new InvalidOperationException("Este presupuesto ya ha sido convertido a pedido.");

        var serie = await _series.GetByCodeAsync("PV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'PV' no encontrada. Configure las series documentales en Configuración.");
        var orderNumber = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var order = SalesOrder.Create(
            orderNumber,
            quote.CustomerId,
            DateOnly.FromDateTime(DateTime.Today),
            command.RequestedDeliveryDate,
            $"Generado desde presupuesto {quote.Number}",
            command.CreatedBy);

        foreach (var line in quote.Lines)
        {
            order.AddLine(
                line.ItemId, line.ItemCode, line.ItemName,
                line.Description,
                line.Quantity,
                line.UnitPrice,
                line.TaxRate);
        }

        await _orders.AddAsync(order, cancellationToken);

        quote.MarkConvertedToOrder(order.Id, command.CreatedBy);
        _quotes.Update(quote);

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _orders.GetByIdAsync(order.Id, cancellationToken);
        return GetSalesOrderByIdHandler.ToDto(saved!);
    }
}
