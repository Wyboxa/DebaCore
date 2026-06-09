using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Name).IsRequired().HasMaxLength(100);
        builder.Property(pm => pm.Code).HasMaxLength(20);
        builder.Property(pm => pm.Description).HasMaxLength(500);
        builder.Property(pm => pm.IsActive).IsRequired();
        builder.Property(pm => pm.CreatedAt).IsRequired();
        builder.Property(pm => pm.CreatedBy).HasMaxLength(100);
        builder.Property(pm => pm.UpdatedBy).HasMaxLength(100);
        builder.Property(pm => pm.DeletedBy).HasMaxLength(100);

        builder.HasIndex(pm => pm.Name).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(pm => pm.Code).IsUnique().HasFilter("[Code] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasQueryFilter(pm => !pm.IsDeleted);
    }
}
