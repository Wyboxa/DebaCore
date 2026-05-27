using Debales.Application.Common;
using Debales.Application.CRM.Activities.DTOs;
using Debales.Application.CRM.Customers;
using Debales.Domain.CRM.Activities;

namespace Debales.Application.CRM.Activities.Commands.LogActivity;

public sealed class LogActivityHandler
{
    private readonly IActivityRepository _activities;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public LogActivityHandler(IActivityRepository activities, ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _activities = activities;
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActivityDto> Handle(LogActivityCommand command, CancellationToken cancellationToken = default)
    {
        var customerExists = await _customers.GetByIdAsync(command.CustomerId, cancellationToken) is not null;
        if (!customerExists)
            throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        var activity = Activity.Log(
            command.CustomerId, command.Type, command.Subject,
            command.ScheduledAt, command.CreatedBy,
            command.ContactId, command.Notes, command.AssignedTo);

        await _activities.AddAsync(activity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(activity);
    }

    internal static ActivityDto ToDto(Activity a) =>
        new(a.Id, a.CustomerId, a.ContactId, a.Type.ToString(), a.Subject,
            a.Notes, a.ScheduledAt, a.CompletedAt, a.IsCompleted, a.AssignedTo, a.CreatedAt, a.CreatedBy);
}
