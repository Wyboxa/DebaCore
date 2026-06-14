using Debales.Application.AIGovernance.DTOs;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIActionProposal;

public sealed class CreateAIActionProposalHandler
{
    private readonly IAIActionProposalRepository _repo;

    public CreateAIActionProposalHandler(IAIActionProposalRepository repo) => _repo = repo;

    public async Task<AIActionProposalDto> Handle(CreateAIActionProposalCommand command, CancellationToken ct = default)
    {
        var proposal = AIActionProposal.Create(
            command.Title, command.ActionType, command.Payload,
            command.Description, command.TargetEntity, command.TargetEntityId,
            command.ProposedByModel, command.CreatedBy);

        await _repo.AddAsync(proposal, ct);
        return ToDto(proposal);
    }

    internal static AIActionProposalDto ToDto(AIActionProposal p) =>
        new(p.Id, p.Title, p.Description, p.ActionType, p.TargetEntity, p.TargetEntityId,
            p.Payload, p.Status.ToString(), p.ProposedByModel, p.ProposedAt, p.RejectionReason, p.CreatedAt);
}
