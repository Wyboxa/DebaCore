namespace Debales.Application.Sales.Commands.UpdatePaymentTerm;

public sealed record UpdatePaymentTermLineRequest(int DueDays, decimal Percentage);

public sealed record UpdatePaymentTermCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    List<UpdatePaymentTermLineRequest> Lines,
    string UpdatedBy);
