using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Code).IsRequired().HasMaxLength(50);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Description).HasMaxLength(2000);
        builder.Property(i => i.IsService).IsRequired();
        builder.Property(i => i.IsActive).IsRequired();
        builder.Property(i => i.SalePrice).IsRequired().HasPrecision(18, 4);
        builder.Property(i => i.PurchasePrice).IsRequired().HasPrecision(18, 4);
        builder.Property(i => i.MinimumStock).HasPrecision(18, 4);
        builder.Property(i => i.CreatedAt).IsRequired();
        builder.Property(i => i.CreatedBy).HasMaxLength(100);
        builder.Property(i => i.UpdatedBy).HasMaxLength(100);
        builder.Property(i => i.DeletedBy).HasMaxLength(100);

        builder.HasIndex(i => i.Code).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.HasOne(i => i.Family)
            .WithMany()
            .HasForeignKey(i => i.FamilyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.UnitOfMeasure)
            .WithMany()
            .HasForeignKey(i => i.UnitOfMeasureId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.TaxType)
            .WithMany()
            .HasForeignKey(i => i.TaxTypeId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
