using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class ItemFamilyConfiguration : IEntityTypeConfiguration<ItemFamily>
{
    public void Configure(EntityTypeBuilder<ItemFamily> builder)
    {
        builder.ToTable("ItemFamilies");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Code).IsRequired().HasMaxLength(20);
        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.IsActive).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.CreatedBy).HasMaxLength(100);
        builder.Property(f => f.UpdatedBy).HasMaxLength(100);
        builder.HasIndex(f => f.Code).IsUnique();
    }
}
