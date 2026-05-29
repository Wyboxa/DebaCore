using Debales.Application.Common;

namespace Debales.Application.Purchasing.Commands.CancelPurchaseOrder;

public sealed class CancelPurchaseOrderHandler
{
    private readonly IPurchaseOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public CancelPurchaseOrderHandler(IPurchaseOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }

    public async Task Handle(CancelPurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Pedido de compra no encontrado.");

        order.Cancel(command.UpdatedBy);
        _orders.Update(order);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
