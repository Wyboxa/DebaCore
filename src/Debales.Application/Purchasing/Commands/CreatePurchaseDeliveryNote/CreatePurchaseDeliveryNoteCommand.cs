namespace Debales.Application.Purchasing.Commands.CreatePurchaseDeliveryNote;

public sealed record CreatePurchaseDeliveryNoteLineRequest(
    Guid? PurchaseOrderLineId,
    Guid? PurchaseOrderId,
    Guid ItemId,
    string? Description,
    decimal Quantity);

public sealed record CreatePurchaseDeliveryNoteCommand(
    Guid SupplierId,
    Guid? PurchaseOrderId,
    DateOnly Date,
    string? Notes,
    IReadOnlyList<CreatePurchaseDeliveryNoteLineRequest> Lines,
    string CreatedBy);
