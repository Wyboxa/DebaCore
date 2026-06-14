namespace Debales.Application.AIGovernance.Commands.UpdateAIRule;

public sealed record UpdateAIRuleCommand(
    Guid Id,
    string Name,
    string ActionType,
    string? Description,
    bool RequiresApproval,
    string? UpdatedBy);
