using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class CustomerItemCodeConfiguration : IEntityTypeConfiguration<CustomerItemCode>
{
    public void Configure(EntityTypeBuilder<CustomerItemCode> builder)
    {
        builder.ToTable("CustomerItemCodes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerCode).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.CreatedBy).HasMaxLength(100);
        builder.Property(c => c.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(c => new { c.CustomerId, c.ItemId }).IsUnique();

        builder.HasOne(c => c.Item)
            .WithMany()
            .HasForeignKey(c => c.ItemId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
