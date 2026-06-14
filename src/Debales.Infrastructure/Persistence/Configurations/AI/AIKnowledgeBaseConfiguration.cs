using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.AI;

public sealed class AIKnowledgeBaseConfiguration : IEntityTypeConfiguration<AIKnowledgeBase>
{
    public void Configure(EntityTypeBuilder<AIKnowledgeBase> builder)
    {
        builder.ToTable("AIKnowledgeBases");

        builder.HasKey(k => k.Id);
        builder.Property(k => k.Title).HasMaxLength(255).IsRequired();
        builder.Property(k => k.Category).HasMaxLength(100);
        builder.Property(k => k.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(k => k.CreatedBy).HasMaxLength(100);
        builder.Property(k => k.UpdatedBy).HasMaxLength(100);
        builder.Property(k => k.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(k => !k.IsDeleted);
        builder.HasIndex(k => k.Category);
    }
}
