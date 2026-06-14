namespace Debales.Application.AIGovernance.Commands.CreateAIRule;

public sealed record CreateAIRuleCommand(
    string Name,
    string ActionType,
    string? Description,
    bool RequiresApproval,
    string? CreatedBy);
