namespace Debales.Application.Sales.Commands.CreateSalesOrder;

public sealed record CreateSalesOrderLineRequest(
    Guid ItemId,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate);

public sealed record CreateSalesOrderCommand(
    Guid CustomerId,
    DateOnly Date,
    DateOnly? RequestedDeliveryDate,
    string? Notes,
    IReadOnlyList<CreateSalesOrderLineRequest> Lines,
    string CreatedBy);
