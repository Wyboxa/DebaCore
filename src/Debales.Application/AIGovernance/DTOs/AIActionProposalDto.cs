namespace Debales.Application.AIGovernance.DTOs;

public sealed record AIActionProposalDto(
    Guid Id,
    string Title,
    string? Description,
    string ActionType,
    string? TargetEntity,
    Guid? TargetEntityId,
    string Payload,
    string Status,
    string? ProposedByModel,
    DateTime ProposedAt,
    string? RejectionReason,
    DateTime CreatedAt);
