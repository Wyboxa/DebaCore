using Debales.Domain.Common;

namespace Debales.Domain.Licensing;

public sealed class SubscriptionPlan : Entity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int MaxUsers { get; private set; }
    public int MaxModules { get; private set; }
    public bool AllowsAI { get; private set; }
    public decimal PriceMonthly { get; private set; }
    public bool IsActive { get; private set; }

    private SubscriptionPlan() { }

    internal static SubscriptionPlan ForSeed(Guid id, string code, string name, string? description,
        int maxUsers, int maxModules, bool allowsAI, decimal priceMonthly) =>
        new()
        {
            Id = id, Code = code, Name = name, Description = description,
            MaxUsers = maxUsers, MaxModules = maxModules, AllowsAI = allowsAI,
            PriceMonthly = priceMonthly, IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = "system"
        };

    public static SubscriptionPlan Create(
        string code,
        string name,
        string? description,
        int maxUsers,
        int maxModules,
        bool allowsAI,
        decimal priceMonthly)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código del plan no puede estar vacío.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del plan no puede estar vacío.", nameof(name));
        if (maxUsers < 0)
            throw new ArgumentException("El número máximo de usuarios no puede ser negativo.", nameof(maxUsers));

        return new SubscriptionPlan
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            Description = description?.Trim(),
            MaxUsers = maxUsers,
            MaxModules = maxModules,
            AllowsAI = allowsAI,
            PriceMonthly = priceMonthly,
            IsActive = true,
            CreatedBy = "system"
        };
    }
}
