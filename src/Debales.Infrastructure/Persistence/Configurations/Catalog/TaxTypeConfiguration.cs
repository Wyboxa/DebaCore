using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Catalog;

internal sealed class TaxTypeConfiguration : IEntityTypeConfiguration<TaxType>
{
    public void Configure(EntityTypeBuilder<TaxType> builder)
    {
        builder.ToTable("TaxTypes");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Code).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Rate).IsRequired().HasPrecision(5, 2);
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.CreatedBy).HasMaxLength(100);
        builder.Property(t => t.UpdatedBy).HasMaxLength(100);
        builder.HasIndex(t => t.Code).IsUnique();
    }
}
