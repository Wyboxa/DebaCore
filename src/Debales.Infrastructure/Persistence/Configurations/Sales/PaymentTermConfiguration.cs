using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class PaymentTermConfiguration : IEntityTypeConfiguration<PaymentTerm>
{
    public void Configure(EntityTypeBuilder<PaymentTerm> builder)
    {
        builder.ToTable("PaymentTerms");
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Name).IsRequired().HasMaxLength(100);
        builder.Property(pt => pt.Description).HasMaxLength(500);
        builder.Property(pt => pt.IsActive).IsRequired();
        builder.Property(pt => pt.CreatedAt).IsRequired();
        builder.Property(pt => pt.CreatedBy).HasMaxLength(100);
        builder.Property(pt => pt.UpdatedBy).HasMaxLength(100);
        builder.Property(pt => pt.DeletedBy).HasMaxLength(100);

        builder.HasIndex(pt => pt.Name).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.HasMany(pt => pt.Lines)
            .WithOne(l => l.PaymentTerm)
            .HasForeignKey(l => l.PaymentTermId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(pt => !pt.IsDeleted);
    }
}
