namespace Debales.Application.Core.Audit.DTOs;

public sealed record AuditEntrySummaryDto(
    Guid Id,
    string EntityName,
    string EntityId,
    string Action,
    string? UserId,
    string? OldValues,
    string? NewValues,
    DateTime CreatedAt);
