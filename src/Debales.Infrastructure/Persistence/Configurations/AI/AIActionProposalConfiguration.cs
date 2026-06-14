using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.AI;

public sealed class AIActionProposalConfiguration : IEntityTypeConfiguration<AIActionProposal>
{
    public void Configure(EntityTypeBuilder<AIActionProposal> builder)
    {
        builder.ToTable("AIActionProposals");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).HasMaxLength(500).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.ActionType).HasMaxLength(100).IsRequired();
        builder.Property(p => p.TargetEntity).HasMaxLength(100);
        builder.Property(p => p.Payload).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.ProposedByModel).HasMaxLength(100);
        builder.Property(p => p.RejectionReason).HasMaxLength(1000);
        builder.Property(p => p.CreatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
        builder.Property(p => p.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(p => !p.IsDeleted);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.ProposedAt);
    }
}
