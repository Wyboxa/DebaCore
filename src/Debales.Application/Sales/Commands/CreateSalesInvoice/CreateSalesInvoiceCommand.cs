namespace Debales.Application.Sales.Commands.CreateSalesInvoice;

public sealed record CreateSalesInvoiceLineRequest(
    Guid ItemId, string? Description, decimal Quantity, decimal UnitPrice, decimal TaxRate);

public sealed record CreateSalesInvoiceCommand(
    Guid CustomerId,
    Guid? SalesDeliveryNoteId,
    DateOnly Date,
    DateOnly DueDate,
    string? Notes,
    IReadOnlyList<CreateSalesInvoiceLineRequest> Lines,
    string CreatedBy);
