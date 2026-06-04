namespace Debales.Application.Licensing.DTOs;

public sealed record SubscriptionPlanDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    int MaxUsers,
    int MaxModules,
    bool AllowsAI,
    decimal PriceMonthly,
    bool IsActive);
