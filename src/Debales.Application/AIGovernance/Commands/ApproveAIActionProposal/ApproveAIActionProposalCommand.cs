namespace Debales.Application.AIGovernance.Commands.ApproveAIActionProposal;

public sealed record ApproveAIActionProposalCommand(Guid Id, string ReviewedBy, string? Notes);
