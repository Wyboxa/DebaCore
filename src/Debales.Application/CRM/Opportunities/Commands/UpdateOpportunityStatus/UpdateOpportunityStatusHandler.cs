using Debales.Application.Common;
using Debales.Application.CRM.Opportunities.Commands.CreateOpportunity;
using Debales.Application.CRM.Opportunities.DTOs;
using Debales.Domain.CRM.Opportunities;

namespace Debales.Application.CRM.Opportunities.Commands.UpdateOpportunityStatus;

public sealed record UpdateOpportunityStatusCommand(Guid CustomerId, Guid OpportunityId, OpportunityStatus NewStatus, string UpdatedBy);

public sealed class UpdateOpportunityStatusHandler
{
    private readonly IOpportunityRepository _opportunities;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOpportunityStatusHandler(IOpportunityRepository opportunities, IUnitOfWork unitOfWork)
    {
        _opportunities = opportunities;
        _unitOfWork = unitOfWork;
    }

    public async Task<OpportunityDto> Handle(UpdateOpportunityStatusCommand command, CancellationToken cancellationToken = default)
    {
        var opportunity = await _opportunities.GetByIdAsync(command.OpportunityId, cancellationToken);

        if (opportunity is null || opportunity.CustomerId != command.CustomerId)
            throw new KeyNotFoundException($"Oportunidad '{command.OpportunityId}' no encontrada.");

        opportunity.UpdateStatus(command.NewStatus, command.UpdatedBy);

        _opportunities.Update(opportunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreateOpportunityHandler.ToDto(opportunity);
    }
}
