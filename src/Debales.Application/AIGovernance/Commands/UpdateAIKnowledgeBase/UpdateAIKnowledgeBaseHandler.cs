using Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;

namespace Debales.Application.AIGovernance.Commands.UpdateAIKnowledgeBase;

public sealed class UpdateAIKnowledgeBaseHandler
{
    private readonly IAIKnowledgeBaseRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAIKnowledgeBaseHandler(IAIKnowledgeBaseRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIKnowledgeBaseDto> Handle(UpdateAIKnowledgeBaseCommand command, CancellationToken ct = default)
    {
        var kb = await _repo.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Entrada de conocimiento no encontrada.");

        kb.Update(command.Title, command.Content, command.Category, command.UpdatedBy);
        _repo.Update(kb);
        await _unitOfWork.SaveChangesAsync(ct);
        return CreateAIKnowledgeBaseHandler.ToDto(kb);
    }
}
