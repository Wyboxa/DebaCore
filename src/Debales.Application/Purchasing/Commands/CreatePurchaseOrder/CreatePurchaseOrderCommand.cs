namespace Debales.Application.Purchasing.Commands.CreatePurchaseOrder;

public sealed record CreatePurchaseOrderLineRequest(
    Guid ItemId,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate);

public sealed record CreatePurchaseOrderCommand(
    Guid SupplierId,
    DateOnly Date,
    DateOnly? ExpectedReceiptDate,
    string? Notes,
    IReadOnlyList<CreatePurchaseOrderLineRequest> Lines,
    string CreatedBy);
