using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class ItemPriceConfiguration : IEntityTypeConfiguration<ItemPrice>
{
    public void Configure(EntityTypeBuilder<ItemPrice> builder)
    {
        builder.ToTable("ItemPrices");
        builder.HasKey(ip => ip.Id);

        builder.Property(ip => ip.Price).IsRequired().HasPrecision(18, 4);
        builder.Property(ip => ip.CreatedAt).IsRequired();
        builder.Property(ip => ip.CreatedBy).HasMaxLength(100);
        builder.Property(ip => ip.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(ip => new { ip.PriceListId, ip.ItemId }).IsUnique();

        builder.HasOne(ip => ip.Item)
            .WithMany()
            .HasForeignKey(ip => ip.ItemId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
