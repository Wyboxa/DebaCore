using Debales.Application.AIGovernance.Commands.CreateAIActionProposal;
using Debales.Application.AIGovernance.DTOs;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.RejectAIActionProposal;

public sealed class RejectAIActionProposalHandler
{
    private readonly IAIActionProposalRepository _proposals;
    private readonly IAIActionApprovalRepository _approvals;

    public RejectAIActionProposalHandler(IAIActionProposalRepository proposals, IAIActionApprovalRepository approvals)
    {
        _proposals = proposals;
        _approvals = approvals;
    }

    public async Task<AIActionProposalDto> Handle(RejectAIActionProposalCommand command, CancellationToken ct = default)
    {
        var proposal = await _proposals.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Propuesta no encontrada.");

        proposal.Reject(command.Reason, command.ReviewedBy);
        _proposals.Update(proposal);

        var approval = AIActionApproval.Create(proposal.Id, AIProposalStatus.Rejected, command.ReviewedBy, command.Notes);
        await _approvals.AddAsync(approval, ct);

        return CreateAIActionProposalHandler.ToDto(proposal);
    }
}
