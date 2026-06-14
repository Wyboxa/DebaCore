using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.AI;

public sealed class AIRuleConfiguration : IEntityTypeConfiguration<AIRule>
{
    public void Configure(EntityTypeBuilder<AIRule> builder)
    {
        builder.ToTable("AIRules");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.ActionType).HasMaxLength(100).IsRequired();
        builder.Property(r => r.CreatedBy).HasMaxLength(100);
        builder.Property(r => r.UpdatedBy).HasMaxLength(100);
        builder.Property(r => r.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(r => !r.IsDeleted);
        builder.HasIndex(r => r.Name).IsUnique().HasFilter("[Name] IS NOT NULL");
    }
}
