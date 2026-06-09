using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderHandler
{
    private readonly IPurchaseOrderRepository _orders;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreatePurchaseOrderHandler(IPurchaseOrderRepository orders, IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _orders = orders;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<PurchaseOrderDetailDto> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("Un pedido debe tener al menos una línea.");

        var serie = await _series.GetByCodeAsync("PC", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'PC' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var order = PurchaseOrder.Create(
            number,
            command.SupplierId,
            command.Date,
            command.ExpectedReceiptDate,
            command.Notes,
            command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            order.AddLine(
                item.Id, item.Code, item.Name,
                lineReq.Description,
                lineReq.Quantity,
                lineReq.UnitPrice,
                lineReq.TaxRate);
        }

        await _orders.AddAsync(order, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _orders.GetByIdAsync(order.Id, cancellationToken);
        return GetPurchaseOrderByIdHandler.ToDto(saved!);
    }
}
