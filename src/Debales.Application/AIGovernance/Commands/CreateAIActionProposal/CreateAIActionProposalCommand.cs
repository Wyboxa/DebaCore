namespace Debales.Application.AIGovernance.Commands.CreateAIActionProposal;

public sealed record CreateAIActionProposalCommand(
    string Title,
    string ActionType,
    string? Payload,
    string? Description,
    string? TargetEntity,
    Guid? TargetEntityId,
    string? ProposedByModel,
    string? CreatedBy);
