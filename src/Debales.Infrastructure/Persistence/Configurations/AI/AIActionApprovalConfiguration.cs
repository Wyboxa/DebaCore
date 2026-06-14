using Debales.Domain.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.AI;

public sealed class AIActionApprovalConfiguration : IEntityTypeConfiguration<AIActionApproval>
{
    public void Configure(EntityTypeBuilder<AIActionApproval> builder)
    {
        builder.ToTable("AIActionApprovals");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Decision).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.Notes).HasMaxLength(2000);
        builder.Property(a => a.ReviewedBy).HasMaxLength(200).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(100);
        builder.Property(a => a.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(a => a.AIActionProposalId);

        builder.HasOne<AIActionProposal>()
            .WithMany()
            .HasForeignKey(a => a.AIActionProposalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
