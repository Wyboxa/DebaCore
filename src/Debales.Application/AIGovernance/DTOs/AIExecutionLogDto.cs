namespace Debales.Application.AIGovernance.DTOs;

public sealed record AIExecutionLogDto(
    Guid Id,
    Guid? AIActionProposalId,
    string ActionType,
    string ActionDescription,
    string ExecutedBy,
    DateTime ExecutedAt,
    bool Success,
    string? ResultSummary,
    string? ErrorMessage);
