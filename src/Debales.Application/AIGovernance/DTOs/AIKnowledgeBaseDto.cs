namespace Debales.Application.AIGovernance.DTOs;

public sealed record AIKnowledgeBaseDto(
    Guid Id,
    string Title,
    string? Category,
    string Content,
    bool IsActive,
    DateTime? LastReviewedAt,
    DateTime CreatedAt);
