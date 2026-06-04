using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;
using Debales.Domain.Inventory;

namespace Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;

public sealed class PostPurchaseDeliveryNoteHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;
    private readonly IPurchaseOrderRepository _orders;
    private readonly IStockMovementRepository _movements;
    private readonly IStockBalanceRepository _balances;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public PostPurchaseDeliveryNoteHandler(
        IPurchaseDeliveryNoteRepository notes,
        IPurchaseOrderRepository orders,
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

    public async Task<PurchaseDeliveryNoteDetailDto> Handle(PostPurchaseDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de compra no encontrado.");

        note.Post(command.UpdatedBy);
        _notes.Update(note);

        // Actualiza cantidades recibidas en el pedido origen si existe
        if (note.PurchaseOrderId.HasValue)
        {
            var order = await _orders.GetByIdAsync(note.PurchaseOrderId.Value, cancellationToken);
            if (order is not null)
            {
                foreach (var noteLine in note.Lines.Where(l => l.PurchaseOrderLineId.HasValue))
                {
                    var orderLine = order.Lines.FirstOrDefault(l => l.Id == noteLine.PurchaseOrderLineId);
                    if (orderLine is not null && noteLine.Quantity > 0)
                    {
                        var canReceive = Math.Min(noteLine.Quantity, orderLine.PendingQuantity);
                        if (canReceive > 0)
                            orderLine.RecordReceipt(canReceive);
                    }
                }
                order.UpdateReceiptStatus(command.UpdatedBy);
                _orders.Update(order);
            }
        }

        // Genera movimientos de stock In por cada línea
        var warehouseId = await ResolveWarehouseAsync(command.WarehouseId, cancellationToken);
        if (warehouseId.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var noteRef = note.Number;

            foreach (var line in note.Lines.Where(l => l.Quantity > 0))
            {
                var number = await _movements.GetNextNumberAsync(cancellationToken);
                var qty = Math.Abs(line.Quantity); // In = positivo

                var movement = StockMovement.Create(
                    number, StockMovementType.In,
                    line.ItemId, line.ItemCode, line.ItemName,
                    warehouseId.Value, null,
                    today, qty,
                    noteRef, $"Albarán compra {noteRef}", command.UpdatedBy);

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
        return GetPurchaseDeliveryNoteByIdHandler.ToDto(saved!);
    }

    private async Task<Guid?> ResolveWarehouseAsync(Guid? requested, CancellationToken ct)
    {
        if (requested.HasValue) return requested;
        var all = await _warehouses.GetAllActiveAsync(ct);
        return all.FirstOrDefault()?.Id;
    }
}
