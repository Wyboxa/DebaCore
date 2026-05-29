namespace Debales.Application.Sales.Commands.CreateSalesDeliveryNote;

public sealed record CreateSalesDeliveryNoteLineRequest(
    Guid? SalesOrderLineId,
    Guid? SalesOrderId,
    Guid ItemId,
    string? Description,
    decimal Quantity);

public sealed record CreateSalesDeliveryNoteCommand(
    Guid CustomerId,
    Guid? SalesOrderId,
    DateOnly Date,
    string? Notes,
    IReadOnlyList<CreateSalesDeliveryNoteLineRequest> Lines,
    string CreatedBy);
