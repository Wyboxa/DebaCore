using Debales.Domain.CRM.Activities;

namespace Debales.Application.CRM.Activities.Commands.LogActivity;

public sealed record LogActivityCommand(
    Guid CustomerId,
    ActivityType Type,
    string Subject,
    DateTime ScheduledAt,
    string CreatedBy,
    Guid? ContactId = null,
    string? Notes = null,
    string? AssignedTo = null);
