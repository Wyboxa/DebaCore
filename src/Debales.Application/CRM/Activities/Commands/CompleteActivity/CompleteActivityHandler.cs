using Debales.Application.Common;
using Debales.Application.CRM.Activities.Commands.LogActivity;
using Debales.Application.CRM.Activities.DTOs;

namespace Debales.Application.CRM.Activities.Commands.CompleteActivity;

public sealed class CompleteActivityHandler
{
    private readonly IActivityRepository _activities;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteActivityHandler(IActivityRepository activities, IUnitOfWork unitOfWork)
    {
        _activities = activities;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActivityDto> Handle(CompleteActivityCommand command, CancellationToken ct = default)
    {
        var activity = await _activities.GetByIdAsync(command.ActivityId, ct);

        if (activity is null || activity.CustomerId != command.CustomerId)
            throw new KeyNotFoundException($"Actividad '{command.ActivityId}' no encontrada.");

        if (activity.IsCompleted)
            throw new InvalidOperationException("La actividad ya está completada.");

        activity.Complete(command.Notes);
        _activities.Update(activity);
        await _unitOfWork.SaveChangesAsync(ct);

        return LogActivityHandler.ToDto(activity);
    }
}
