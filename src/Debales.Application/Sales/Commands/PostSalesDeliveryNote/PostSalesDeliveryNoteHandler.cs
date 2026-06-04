using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Domain.Inventory;

namespace Debales.Application.Sales.Commands.PostSalesDeliveryNote;

public sealed class PostSalesDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly ISalesOrderRepository _orders;
    private readonly IStockMovementRepository _movements;
    private readonly IStockBalanceRepository _balances;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public PostSalesDeliveryNoteHandler(
        ISalesDeliveryNoteRepository notes,
        ISalesOrderRepository orders,
        IStockMovementRepository movements,
        IStockBalanceRepository balances,
        IWarehouseRepository warehouses,
        IUnitOfWork uow)
    {
        _notes = notes;
        _orders = orders;
        _movements = movements;
        _balances = balances;
        _warehouses = warehouses;
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

        // Genera movimientos de stock Out por cada línea
        var warehouseId = await ResolveWarehouseAsync(command.WarehouseId, cancellationToken);
        if (warehouseId.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var noteRef = note.Number;

            foreach (var line in note.Lines.Where(l => l.Quantity > 0))
            {
                var number = await _movements.GetNextNumberAsync(cancellationToken);
                var qty = -Math.Abs(line.Quantity); // Out = negativo

                var movement = StockMovement.Create(
                    number, StockMovementType.Out,
                    line.ItemId, line.ItemCode, line.ItemName,
                    warehouseId.Value, null,
                    today, qty,
                    noteRef, $"Albarán venta {noteRef}", command.UpdatedBy);

                await _movements.AddAsync(movement, cancellationToken);

                var balance = await _balances.GetAsync(line.ItemId, warehouseId.Value, cancellationToken);
                if (balance is null)
                {
                    balance = StockBalance.Create(line.ItemId, warehouseId.Value);
                    balance.Apply(qty);
                    await _balances.AddAsync(balance, cancellationToken);
                }
                else
                {
                    balance.Apply(qty);
                }
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesDeliveryNoteByIdHandler.ToDto(saved!);
    }

    private async Task<Guid?> ResolveWarehouseAsync(Guid? requested, CancellationToken ct)
    {
        if (requested.HasValue) return requested;
        var all = await _warehouses.GetAllActiveAsync(ct);
        return all.FirstOrDefault()?.Id;
    }
}
