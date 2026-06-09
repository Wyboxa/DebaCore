namespace Debales.Application.Sales.Commands.UpdatePaymentMethod;

public sealed record UpdatePaymentMethodCommand(Guid Id, string Name, string? Code, string? Description, bool IsActive, string UpdatedBy);
