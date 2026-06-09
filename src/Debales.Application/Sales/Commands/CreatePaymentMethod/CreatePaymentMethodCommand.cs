namespace Debales.Application.Sales.Commands.CreatePaymentMethod;

public sealed record CreatePaymentMethodCommand(string Name, string? Code, string? Description, string CreatedBy);
