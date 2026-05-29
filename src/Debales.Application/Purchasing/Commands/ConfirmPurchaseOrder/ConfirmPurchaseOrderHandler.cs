using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;

namespace Debales.Application.Purchasing.Commands.ConfirmPurchaseOrder;

public sealed class ConfirmPurchaseOrderHandler
{
    private readonly IPurchaseOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public ConfirmPurchaseOrderHandler(IPurchaseOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }

    public async Task<PurchaseOrderDetailDto> Handle(ConfirmPurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Pedido de compra no encontrado.");

        order.Confirm(command.UpdatedBy);
        _orders.Update(order);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _orders.GetByIdAsync(order.Id, cancellationToken);
        return GetPurchaseOrderByIdHandler.ToDto(saved!);
    }
}
