namespace Debales.Application.AIGovernance.DTOs;

public sealed record AIActionApprovalDto(
    Guid Id,
    Guid AIActionProposalId,
    string Decision,
    string? Notes,
    string ReviewedBy,
    DateTime ReviewedAt);
