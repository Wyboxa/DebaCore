namespace Debales.Application.AIGovernance.Commands.RejectAIActionProposal;

public sealed record RejectAIActionProposalCommand(Guid Id, string Reason, string ReviewedBy, string? Notes);
