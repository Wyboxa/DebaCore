namespace Debales.Application.Sales.Commands.CreateCustomerPayment;

public sealed record CreateCustomerPaymentCommand(
    Guid CustomerId,
    Guid? ReceivableId,
    DateOnly Date,
    decimal Amount,
    string? Reference,
    string? Notes,
    string CreatedBy);
