using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIActionProposal;

public sealed class CreateAIActionProposalHandler
{
    private readonly IAIActionProposalRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAIActionProposalHandler(IAIActionProposalRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIActionProposalDto> Handle(CreateAIActionProposalCommand command, CancellationToken ct = default)
    {
        var proposal = AIActionProposal.Create(
            command.Title, command.ActionType, command.Payload,
            command.Description, command.TargetEntity, command.TargetEntityId,
            command.ProposedByModel, command.CreatedBy);

        await _repo.AddAsync(proposal, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return ToDto(proposal);
    }

    internal static AIActionProposalDto ToDto(AIActionProposal p) =>
        new(p.Id, p.Title, p.Description, p.ActionType, p.TargetEntity, p.TargetEntityId,
            p.Payload, p.Status.ToString(), p.ProposedByModel, p.ProposedAt, p.RejectionReason, p.CreatedAt);
}
