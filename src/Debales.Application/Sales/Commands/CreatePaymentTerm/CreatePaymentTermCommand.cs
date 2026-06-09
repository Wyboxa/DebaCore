namespace Debales.Application.Sales.Commands.CreatePaymentTerm;

public sealed record CreatePaymentTermLineRequest(int DueDays, decimal Percentage);

public sealed record CreatePaymentTermCommand(
    string Name,
    string? Description,
    List<CreatePaymentTermLineRequest> Lines,
    string CreatedBy);
