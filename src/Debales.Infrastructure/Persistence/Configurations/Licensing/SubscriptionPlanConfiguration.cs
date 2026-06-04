using Debales.Domain.Licensing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Licensing;

internal sealed class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("SubscriptionPlans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.Code).IsUnique();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.MaxUsers).IsRequired();
        builder.Property(p => p.MaxModules).IsRequired();
        builder.Property(p => p.AllowsAI).IsRequired();
        builder.Property(p => p.PriceMonthly).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.CreatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);

        builder.HasData(
            SubscriptionPlan.ForSeed(
                new Guid("10000000-0000-0000-0000-000000000001"),
                "TRIAL", "Trial", "Acceso completo por 30 días. Sin compromiso.",
                maxUsers: 3, maxModules: 99, allowsAI: true, priceMonthly: 0m),
            SubscriptionPlan.ForSeed(
                new Guid("10000000-0000-0000-0000-000000000002"),
                "STARTER", "Starter", "Hasta 5 usuarios. CRM + ERP básico. Sin IA.",
                maxUsers: 5, maxModules: 5, allowsAI: false, priceMonthly: 49m),
            SubscriptionPlan.ForSeed(
                new Guid("10000000-0000-0000-0000-000000000003"),
                "PROFESSIONAL", "Professional", "Hasta 20 usuarios. Todos los módulos. IA incluida.",
                maxUsers: 20, maxModules: 99, allowsAI: true, priceMonthly: 149m)
        );
    }
}
