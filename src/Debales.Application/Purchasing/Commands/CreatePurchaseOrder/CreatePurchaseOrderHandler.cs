using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderHandler
{
    private readonly IPurchaseOrderRepository _orders;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreatePurchaseOrderHandler(IPurchaseOrderRepository orders, IItemRepository items, IUnitOfWork uow)
    {
        _orders = orders;
        _items = items;
        _uow = uow;
    }

    public async Task<PurchaseOrderDetailDto> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("Un pedido debe tener al menos una línea.");

        var number = await _orders.GetNextNumberAsync(cancellationToken);

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
