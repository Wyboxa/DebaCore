namespace Debales.Application.Purchasing.Commands.CreatePurchaseInvoice;

public sealed record CreatePurchaseInvoiceLineRequest(
    Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

public sealed record CreatePurchaseInvoiceCommand(
    string? SupplierInvoiceNumber,
    Guid SupplierId,
    Guid? PurchaseDeliveryNoteId,
    DateOnly Date,
    DateOnly DueDate,
    string? Notes,
    IReadOnlyList<CreatePurchaseInvoiceLineRequest> Lines,
    string CreatedBy);
