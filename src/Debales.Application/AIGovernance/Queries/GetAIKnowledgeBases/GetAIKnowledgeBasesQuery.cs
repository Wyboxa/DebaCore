namespace Debales.Application.AIGovernance.Queries.GetAIKnowledgeBases;

public sealed record GetAIKnowledgeBasesQuery(string? Search, int Page = 1, int PageSize = 20);
