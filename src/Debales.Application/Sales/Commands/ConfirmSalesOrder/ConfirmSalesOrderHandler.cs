using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesOrderById;

namespace Debales.Application.Sales.Commands.ConfirmSalesOrder;

public sealed class ConfirmSalesOrderHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public ConfirmSalesOrderHandler(ISalesOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }

    public async Task<SalesOrderDetailDto> Handle(ConfirmSalesOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Pedido de venta no encontrado.");

        order.Confirm(command.UpdatedBy);
        _orders.Update(order);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _orders.GetByIdAsync(order.Id, cancellationToken);
        return GetSalesOrderByIdHandler.ToDto(saved!);
    }
}
