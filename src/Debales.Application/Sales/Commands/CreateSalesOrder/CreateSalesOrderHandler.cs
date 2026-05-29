using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesOrderById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesOrder;

public sealed class CreateSalesOrderHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreateSalesOrderHandler(ISalesOrderRepository orders, IItemRepository items, IUnitOfWork uow)
    {
        _orders = orders;
        _items = items;
        _uow = uow;
    }

    public async Task<SalesOrderDetailDto> Handle(CreateSalesOrderCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("An order must have at least one line.");

        var number = await _orders.GetNextNumberAsync(cancellationToken);

        var order = SalesOrder.Create(
            number,
            command.CustomerId,
            command.Date,
            command.RequestedDeliveryDate,
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
        return GetSalesOrderByIdHandler.ToDto(saved!);
    }
}
