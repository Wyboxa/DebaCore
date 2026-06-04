namespace Debales.Application.Sales.Commands.ConvertQuoteToOrder;

public sealed record ConvertQuoteToOrderCommand(
    Guid QuoteId,
    DateOnly? RequestedDeliveryDate,
    string CreatedBy);
