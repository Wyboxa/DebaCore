namespace Debales.Application.Purchasing.Commands.GenerateInvoiceFromPurchaseDeliveryNote;

public sealed record GenerateInvoiceFromPurchaseDeliveryNoteCommand(
    Guid PurchaseDeliveryNoteId,
    DateOnly DueDate,
    string CreatedBy);
