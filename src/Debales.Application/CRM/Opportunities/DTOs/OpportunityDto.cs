namespace Debales.Application.CRM.Opportunities.DTOs;

public sealed record OpportunityDto(
    Guid Id,
    Guid CustomerId,
    string Title,
    decimal? EstimatedValue,
    string Status,
    DateTime? ExpectedCloseDate,
    DateTime CreatedAt,
    string? CreatedBy);
