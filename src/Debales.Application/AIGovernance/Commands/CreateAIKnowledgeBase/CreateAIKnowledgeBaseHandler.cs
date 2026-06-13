using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;

public sealed class CreateAIKnowledgeBaseHandler
{
    private readonly IAIKnowledgeBaseRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAIKnowledgeBaseHandler(IAIKnowledgeBaseRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIKnowledgeBaseDto> Handle(CreateAIKnowledgeBaseCommand command, CancellationToken ct = default)
    {
        var kb = AIKnowledgeBase.Create(command.Title, command.Content, command.Category, command.CreatedBy);
        await _repo.AddAsync(kb, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return ToDto(kb);
    }

    internal static AIKnowledgeBaseDto ToDto(AIKnowledgeBase kb) =>
        new(kb.Id, kb.Title, kb.Category, kb.Content, kb.IsActive, kb.LastReviewedAt, kb.CreatedAt);
}
