namespace Debales.Application.Sales.Commands.CreateSalesQuote;

public sealed record CreateSalesQuoteCommand(
    Guid CustomerId,
    DateOnly Date,
    DateOnly ValidUntil,
    string? Notes,
    IReadOnlyList<CreateSalesQuoteLineRequest> Lines,
    string CreatedBy);

public sealed record CreateSalesQuoteLineRequest(
    Guid ItemId,
    string? Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate);
