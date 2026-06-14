using Debales.Application.AIGovernance.Commands.CreateAIActionProposal;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;

namespace Debales.Application.AIGovernance.Queries.GetAIActionProposals;

public sealed class GetAIActionProposalsHandler
{
    private readonly IAIActionProposalRepository _repo;

    public GetAIActionProposalsHandler(IAIActionProposalRepository repo) => _repo = repo;

    public async Task<PagedResult<AIActionProposalDto>> Handle(GetAIActionProposalsQuery query, CancellationToken ct = default)
    {
        var paged = await _repo.SearchAsync(query.Status, query.Page, query.PageSize, ct);
        var dtos = paged.Items.Select(CreateAIActionProposalHandler.ToDto).ToList();
        return new PagedResult<AIActionProposalDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
