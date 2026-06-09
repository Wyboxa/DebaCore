using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class PriceListConfiguration : IEntityTypeConfiguration<PriceList>
{
    public void Configure(EntityTypeBuilder<PriceList> builder)
    {
        builder.ToTable("PriceLists");
        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.Name).IsRequired().HasMaxLength(100);
        builder.Property(pl => pl.Description).HasMaxLength(500);
        builder.Property(pl => pl.IsActive).IsRequired();
        builder.Property(pl => pl.ValidFrom).IsRequired(false);
        builder.Property(pl => pl.ValidTo).IsRequired(false);
        builder.Property(pl => pl.CreatedAt).IsRequired();
        builder.Property(pl => pl.CreatedBy).HasMaxLength(100);
        builder.Property(pl => pl.UpdatedBy).HasMaxLength(100);
        builder.Property(pl => pl.DeletedBy).HasMaxLength(100);

        builder.HasIndex(pl => pl.Name).HasFilter("[IsDeleted] = 0");

        builder.HasMany(pl => pl.Items)
            .WithOne(ip => ip.PriceList)
            .HasForeignKey(ip => ip.PriceListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(pl => !pl.IsDeleted);
    }
}
