namespace Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;

public sealed record CreateAIKnowledgeBaseCommand(
    string Title,
    string Content,
    string? Category,
    string? CreatedBy);
