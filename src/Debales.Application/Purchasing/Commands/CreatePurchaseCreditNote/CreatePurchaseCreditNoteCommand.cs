namespace Debales.Application.Purchasing.Commands.CreatePurchaseCreditNote;

public sealed record CreatePurchaseCreditNoteLineRequest(
    Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

public sealed record CreatePurchaseCreditNoteCommand(
    Guid SupplierId,
    Guid OriginalInvoiceId,
    DateOnly Date,
    string? Reason,
    IReadOnlyList<CreatePurchaseCreditNoteLineRequest> Lines,
    string CreatedBy);
