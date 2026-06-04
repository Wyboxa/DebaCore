namespace Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;

public sealed record PostPurchaseDeliveryNoteCommand(Guid Id, string UpdatedBy, Guid? WarehouseId = null);
