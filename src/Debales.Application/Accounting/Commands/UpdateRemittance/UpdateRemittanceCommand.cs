namespace Debales.Application.Accounting.Commands.UpdateRemittance;

public sealed record UpdateRemittanceCommand(Guid Id, string? Notes, string UpdatedBy);
