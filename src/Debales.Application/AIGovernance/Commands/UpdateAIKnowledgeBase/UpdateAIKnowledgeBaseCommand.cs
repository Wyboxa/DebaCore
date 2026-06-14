namespace Debales.Application.AIGovernance.Commands.UpdateAIKnowledgeBase;

public sealed record UpdateAIKnowledgeBaseCommand(
    Guid Id,
    string Title,
    string Content,
    string? Category,
    string? UpdatedBy);
