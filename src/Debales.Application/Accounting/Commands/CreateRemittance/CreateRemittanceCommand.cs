using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateRemittance;

public sealed record CreateRemittanceCommand(
    DateOnly Date, Guid BankAccountId, RemittanceType Type, string? Notes, string CreatedBy);
