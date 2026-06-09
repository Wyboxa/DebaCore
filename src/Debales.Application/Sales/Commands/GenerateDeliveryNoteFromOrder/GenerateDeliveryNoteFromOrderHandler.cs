using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.GenerateDeliveryNoteFromOrder;

public sealed class GenerateDeliveryNoteFromOrderHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public GenerateDeliveryNoteFromOrderHandler(
        ISalesOrderRepository orders,
        ISalesDeliveryNoteRepository notes,
        INumberSeriesRepository series,
        IUnitOfWork uow)
    {
        _orders = orders;
        _notes = notes;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesDeliveryNoteDetailDto> Handle(
        GenerateDeliveryNoteFromOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.SalesOrderId, cancellationToken)
            ?? throw new InvalidOperationException("Pedido de venta no encontrado.");

        if (order.Status != SalesOrderStatus.Confirmed && order.Status != SalesOrderStatus.PartiallyDelivered)
            throw new InvalidOperationException($"No se puede generar albarán desde un pedido en estado '{order.Status}'.");

        var pendingLines = order.Lines.Where(l => l.PendingQuantity > 0).ToList();
        if (pendingLines.Count == 0)
            throw new InvalidOperationException("El pedido no tiene líneas pendientes de entregar.");

        var serie = await _series.GetByCodeAsync("ALV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'ALV' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);
        var note = SalesDeliveryNote.Create(
            number,
            order.CustomerId,
            order.Id,
            DateOnly.FromDateTime(DateTime.Today),
            $"Generado desde pedido {order.Number}",
            command.CreatedBy);

        foreach (var line in pendingLines)
        {
            note.AddLine(
                line.Id,
                order.Id,
                line.ItemId, line.ItemCode, line.ItemName,
                line.Description,
                line.PendingQuantity);
        }

        await _notes.AddAsync(note, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
