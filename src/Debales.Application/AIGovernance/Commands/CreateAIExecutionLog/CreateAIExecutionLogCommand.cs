namespace Debales.Application.AIGovernance.Commands.CreateAIExecutionLog;

public sealed record CreateAIExecutionLogCommand(
    string ActionType,
    string ActionDescription,
    string ExecutedBy,
    bool Success,
    string? ResultSummary,
    string? ErrorMessage,
    Guid? AIActionProposalId);
