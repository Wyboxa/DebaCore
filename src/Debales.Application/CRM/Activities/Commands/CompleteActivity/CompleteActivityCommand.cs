namespace Debales.Application.CRM.Activities.Commands.CompleteActivity;

public sealed record CompleteActivityCommand(Guid CustomerId, Guid ActivityId, string? Notes, string UpdatedBy);
