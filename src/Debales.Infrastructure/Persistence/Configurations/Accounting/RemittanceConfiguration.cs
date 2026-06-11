using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class RemittanceConfiguration : IEntityTypeConfiguration<Remittance>
{
    public void Configure(EntityTypeBuilder<Remittance> builder)
    {
        builder.ToTable("Remittances");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Number).IsRequired().HasMaxLength(30);
        builder.Property(r => r.Notes).HasMaxLength(500);
        builder.Property(r => r.FailReason).HasMaxLength(500);

        builder.HasIndex(r => r.Number).IsUnique();
        builder.HasQueryFilter(r => !r.IsDeleted);

        builder.HasOne(r => r.BankAccount)
            .WithMany()
            .HasForeignKey(r => r.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Lines)
            .WithOne(l => l.Remittance)
            .HasForeignKey(l => l.RemittanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
