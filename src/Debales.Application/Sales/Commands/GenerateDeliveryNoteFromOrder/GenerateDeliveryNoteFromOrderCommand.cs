namespace Debales.Application.Sales.Commands.GenerateDeliveryNoteFromOrder;

public sealed record GenerateDeliveryNoteFromOrderCommand(
    Guid SalesOrderId,
    string CreatedBy);
