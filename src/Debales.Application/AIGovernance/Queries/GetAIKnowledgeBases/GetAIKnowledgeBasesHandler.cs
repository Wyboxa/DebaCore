using Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;

namespace Debales.Application.AIGovernance.Queries.GetAIKnowledgeBases;

public sealed class GetAIKnowledgeBasesHandler
{
    private readonly IAIKnowledgeBaseRepository _repo;

    public GetAIKnowledgeBasesHandler(IAIKnowledgeBaseRepository repo) => _repo = repo;

    public async Task<PagedResult<AIKnowledgeBaseDto>> Handle(GetAIKnowledgeBasesQuery query, CancellationToken ct = default)
    {
        var paged = await _repo.SearchAsync(query.Search, query.Page, query.PageSize, ct);
        var dtos = paged.Items.Select(CreateAIKnowledgeBaseHandler.ToDto).ToList();
        return new PagedResult<AIKnowledgeBaseDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
