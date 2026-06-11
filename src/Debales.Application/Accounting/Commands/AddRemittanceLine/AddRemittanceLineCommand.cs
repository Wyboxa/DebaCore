namespace Debales.Application.Accounting.Commands.AddRemittanceLine;

public sealed record AddRemittanceLineCommand(Guid RemittanceId, Guid DocumentId, decimal Amount, string UpdatedBy);
