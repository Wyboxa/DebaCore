namespace Debales.Application.Sales.Commands.GenerateInvoiceFromDeliveryNote;

public sealed record GenerateInvoiceFromDeliveryNoteCommand(
    Guid SalesDeliveryNoteId,
    DateOnly DueDate,
    string CreatedBy);
