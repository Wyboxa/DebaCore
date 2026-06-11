namespace Debales.Application.Accounting.Commands.FailRemittance;

public sealed record FailRemittanceCommand(Guid Id, string Reason, string UpdatedBy);
