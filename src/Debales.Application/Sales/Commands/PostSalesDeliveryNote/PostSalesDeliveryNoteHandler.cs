using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;

namespace Debales.Application.Sales.Commands.PostSalesDeliveryNote;

public sealed class PostSalesDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly ISalesOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public PostSalesDeliveryNoteHandler(ISalesDeliveryNoteRepository notes, ISalesOrderRepository orders, IUnitOfWork uow)
    {
        _notes = notes;
        _orders = orders;
        _uow = uow;
    }

    public async Task<SalesDeliveryNoteDetailDto> Handle(PostSalesDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de venta no encontrado.");

        note.Post(command.UpdatedBy);
        _notes.Update(note);

        // Actualiza cantidades entregadas en el pedido origen si existe
        if (note.SalesOrderId.HasValue)
        {
            var order = await _orders.GetByIdAsync(note.SalesOrderId.Value, cancellationToken);
            if (order is not null)
            {
                foreach (var noteLine in note.Lines.Where(l => l.SalesOrderLineId.HasValue))
                {
                    var orderLine = order.Lines.FirstOrDefault(l => l.Id == noteLine.SalesOrderLineId);
                    if (orderLine is not null && noteLine.Quantity > 0)
                    {
                        var canDeliver = Math.Min(noteLine.Quantity, orderLine.PendingQuantity);
                        if (canDeliver > 0)
                            orderLine.RecordDelivery(canDeliver);
                    }
                }
                order.UpdateDeliveryStatus(command.UpdatedBy);
                _orders.Update(order);
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
