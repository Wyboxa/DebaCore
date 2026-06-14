namespace Debales.Application.AIGovernance.DTOs;

public sealed record AIRuleDto(
    Guid Id,
    string Name,
    string? Description,
    string ActionType,
    bool RequiresApproval,
    bool IsActive,
    DateTime CreatedAt);
