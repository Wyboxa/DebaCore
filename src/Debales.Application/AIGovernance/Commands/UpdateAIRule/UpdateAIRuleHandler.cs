using Debales.Application.AIGovernance.Commands.CreateAIRule;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;

namespace Debales.Application.AIGovernance.Commands.UpdateAIRule;

public sealed class UpdateAIRuleHandler
{
    private readonly IAIRuleRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAIRuleHandler(IAIRuleRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIRuleDto> Handle(UpdateAIRuleCommand command, CancellationToken ct = default)
    {
        var rule = await _repo.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Regla no encontrada.");

        rule.Update(command.Name, command.ActionType, command.Description, command.RequiresApproval, command.UpdatedBy);
        _repo.Update(rule);
        await _unitOfWork.SaveChangesAsync(ct);
        return CreateAIRuleHandler.ToDto(rule);
    }
}
