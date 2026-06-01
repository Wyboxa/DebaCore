namespace Debales.Application.Purchasing.Commands.CreateSupplierPayment;

public sealed record CreateSupplierPaymentCommand(
    Guid SupplierId,
    Guid? PayableId,
    DateOnly Date,
    decimal Amount,
    string? Reference,
    string? Notes,
    string CreatedBy);
