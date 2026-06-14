using Debales.Application.AIGovernance.Commands.CreateAIActionProposal;
using Debales.Application.AIGovernance.DTOs;

namespace Debales.Application.AIGovernance.Queries.GetAIActionProposalById;

public sealed class GetAIActionProposalByIdHandler
{
    private readonly IAIActionProposalRepository _proposals;
    private readonly IAIActionApprovalRepository _approvals;

    public GetAIActionProposalByIdHandler(IAIActionProposalRepository proposals, IAIActionApprovalRepository approvals)
    {
        _proposals = proposals;
        _approvals = approvals;
    }

    public async Task<(AIActionProposalDto Proposal, IReadOnlyList<AIActionApprovalDto> Approvals)?> Handle(
        GetAIActionProposalByIdQuery query, CancellationToken ct = default)
    {
        var proposal = await _proposals.GetByIdAsync(query.Id, ct);
        if (proposal is null) return null;

        var approvals = await _approvals.GetByProposalIdAsync(query.Id, ct);
        var approvalDtos = approvals.Select(a =>
            new AIActionApprovalDto(a.Id, a.AIActionProposalId, a.Decision.ToString(), a.Notes, a.ReviewedBy, a.ReviewedAt))
            .ToList();

        return (CreateAIActionProposalHandler.ToDto(proposal), approvalDtos);
    }
}
