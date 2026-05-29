using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.CancelSalesOrder;

public sealed class CancelSalesOrderHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public CancelSalesOrderHandler(ISalesOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }

    public async Task Handle(CancelSalesOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Pedido de venta no encontrado.");

        order.Cancel(command.UpdatedBy);
        _orders.Update(order);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
