namespace Debales.Application.Sales.Commands.PostSalesDeliveryNote;

public sealed record PostSalesDeliveryNoteCommand(Guid Id, string UpdatedBy, Guid? WarehouseId = null);
