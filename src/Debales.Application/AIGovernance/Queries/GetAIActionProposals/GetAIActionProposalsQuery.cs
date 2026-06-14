namespace Debales.Application.AIGovernance.Queries.GetAIActionProposals;

public sealed record GetAIActionProposalsQuery(string? Status, int Page = 1, int PageSize = 20);
