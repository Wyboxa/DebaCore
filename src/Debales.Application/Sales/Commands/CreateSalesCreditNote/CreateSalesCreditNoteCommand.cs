namespace Debales.Application.Sales.Commands.CreateSalesCreditNote;

public sealed record CreateSalesCreditNoteLineRequest(
    Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

public sealed record CreateSalesCreditNoteCommand(
    Guid CustomerId,
    Guid OriginalInvoiceId,
    DateOnly Date,
    string? Reason,
    IReadOnlyList<CreateSalesCreditNoteLineRequest> Lines,
    string CreatedBy);
