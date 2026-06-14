using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.AI;

public sealed class AIExecutionLogConfiguration : IEntityTypeConfiguration<AIExecutionLog>
{
    public void Configure(EntityTypeBuilder<AIExecutionLog> builder)
    {
        builder.ToTable("AIExecutionLogs");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.ActionType).HasMaxLength(100).IsRequired();
        builder.Property(l => l.ActionDescription).HasMaxLength(2000).IsRequired();
        builder.Property(l => l.ExecutedBy).HasMaxLength(200).IsRequired();
        builder.Property(l => l.ResultSummary).HasMaxLength(2000);
        builder.Property(l => l.ErrorMessage).HasMaxLength(2000);
        builder.Property(l => l.CreatedBy).HasMaxLength(100);
        builder.Property(l => l.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(l => l.ExecutedAt);
        builder.HasIndex(l => l.AIActionProposalId);
    }
}
