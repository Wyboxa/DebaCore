using Debales.Application.AIGovernance.Commands.CreateAIActionProposal;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.ApproveAIActionProposal;

public sealed class ApproveAIActionProposalHandler
{
    private readonly IAIActionProposalRepository _proposals;
    private readonly IAIActionApprovalRepository _approvals;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveAIActionProposalHandler(
        IAIActionProposalRepository proposals,
        IAIActionApprovalRepository approvals,
        IUnitOfWork unitOfWork)
    {
        _proposals = proposals;
        _approvals = approvals;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIActionProposalDto> Handle(ApproveAIActionProposalCommand command, CancellationToken ct = default)
    {
        var proposal = await _proposals.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Propuesta no encontrada.");

        proposal.Approve(command.ReviewedBy);
        _proposals.Update(proposal);

        var approval = AIActionApproval.Create(proposal.Id, AIProposalStatus.Approved, command.ReviewedBy, command.Notes);
        await _approvals.AddAsync(approval, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return CreateAIActionProposalHandler.ToDto(proposal);
    }
}
