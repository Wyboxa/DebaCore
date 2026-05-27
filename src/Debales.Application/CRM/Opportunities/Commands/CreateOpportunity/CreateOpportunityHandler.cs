using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Opportunities.DTOs;
using Debales.Domain.CRM.Opportunities;

namespace Debales.Application.CRM.Opportunities.Commands.CreateOpportunity;

public sealed record CreateOpportunityCommand(
    Guid CustomerId,
    string Title,
    string CreatedBy,
    decimal? EstimatedValue = null,
    DateTime? ExpectedCloseDate = null);

public sealed class CreateOpportunityHandler
{
    private readonly IOpportunityRepository _opportunities;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOpportunityHandler(IOpportunityRepository opportunities, ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _opportunities = opportunities;
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<OpportunityDto> Handle(CreateOpportunityCommand command, CancellationToken cancellationToken = default)
    {
        var customerExists = await _customers.GetByIdAsync(command.CustomerId, cancellationToken) is not null;
        if (!customerExists)
            throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        var opportunity = Opportunity.Create(
            command.CustomerId, command.Title, command.CreatedBy,
            command.EstimatedValue, command.ExpectedCloseDate);

        await _opportunities.AddAsync(opportunity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(opportunity);
    }

    internal static OpportunityDto ToDto(Opportunity o) =>
        new(o.Id, o.CustomerId, o.Title, o.EstimatedValue, o.Status.ToString(), o.ExpectedCloseDate, o.CreatedAt, o.CreatedBy);
}
