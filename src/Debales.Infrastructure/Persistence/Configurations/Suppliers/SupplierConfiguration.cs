using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Suppliers;

internal sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.TaxId).HasMaxLength(20);
        builder.HasIndex(s => s.TaxId).IsUnique().HasFilter("[TaxId] IS NOT NULL");
        builder.Property(s => s.Phone).HasMaxLength(30);
        builder.Property(s => s.Email).HasMaxLength(150);
        builder.Property(s => s.Website).HasMaxLength(255);
        builder.Property(s => s.ContactName).HasMaxLength(150);
        builder.Property(s => s.Notes).HasMaxLength(2000);
        builder.Property(s => s.IsActive).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.CreatedBy).HasMaxLength(100);
        builder.Property(s => s.UpdatedBy).HasMaxLength(100);
        builder.Property(s => s.DeletedBy).HasMaxLength(100);

        builder.OwnsOne(s => s.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("AddressStreet").HasMaxLength(255);
            address.Property(a => a.City).HasColumnName("AddressCity").HasMaxLength(100);
            address.Property(a => a.PostalCode).HasColumnName("AddressPostalCode").HasMaxLength(20);
            address.Property(a => a.Country).HasColumnName("AddressCountry").HasMaxLength(100);
        });

        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
