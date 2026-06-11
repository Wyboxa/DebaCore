namespace Debales.Application.Accounting.Commands.RemoveRemittanceLine;

public sealed record RemoveRemittanceLineCommand(Guid RemittanceId, Guid DocumentId, string UpdatedBy);
