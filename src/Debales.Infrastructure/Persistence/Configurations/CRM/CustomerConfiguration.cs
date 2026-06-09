using Debales.Domain.CRM.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.CRM;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("CrmCustomers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Sector).HasMaxLength(100);
        builder.Property(c => c.TaxId).HasMaxLength(20);
        builder.HasIndex(c => c.TaxId).IsUnique().HasFilter("[TaxId] IS NOT NULL");
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.Website).HasMaxLength(255);
        builder.Property(c => c.IsActive).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.CreatedBy).HasMaxLength(100);
        builder.Property(c => c.UpdatedBy).HasMaxLength(100);
        builder.Property(c => c.DeletedBy).HasMaxLength(100);

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("AddressStreet").HasMaxLength(255);
            address.Property(a => a.City).HasColumnName("AddressCity").HasMaxLength(100);
            address.Property(a => a.PostalCode).HasColumnName("AddressPostalCode").HasMaxLength(20);
            address.Property(a => a.Country).HasColumnName("AddressCountry").HasMaxLength(100);
        });

        builder.Property(c => c.AccountCode).HasMaxLength(20);
        builder.Property(c => c.PriceListId);
        builder.HasOne<Debales.Domain.Catalog.PriceList>().WithMany().HasForeignKey(c => c.PriceListId)
            .OnDelete(DeleteBehavior.SetNull).IsRequired(false);
        builder.Property(c => c.PaymentTermId);
        builder.HasOne<Debales.Domain.Sales.PaymentTerm>().WithMany().HasForeignKey(c => c.PaymentTermId)
            .OnDelete(DeleteBehavior.SetNull).IsRequired(false);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasMany(c => c.Contacts).WithOne().HasForeignKey(ct => ct.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Activities).WithOne().HasForeignKey(a => a.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Notes).WithOne().HasForeignKey(n => n.CustomerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(c => c.Opportunities).WithOne().HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Restrict);
    }
}
