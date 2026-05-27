using Debales.Domain.CRM.Activities;

namespace Debales.Application.CRM.Activities.DTOs;

public sealed record ActivityDto(
    Guid Id,
    Guid CustomerId,
    Guid? ContactId,
    string Type,
    string Subject,
    string? Notes,
    DateTime ScheduledAt,
    DateTime? CompletedAt,
    bool IsCompleted,
    string? AssignedTo,
    DateTime CreatedAt,
    string? CreatedBy);
