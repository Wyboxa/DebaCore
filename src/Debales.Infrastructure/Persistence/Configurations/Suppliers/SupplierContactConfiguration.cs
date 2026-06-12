using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Suppliers;

internal sealed class SupplierContactConfiguration : IEntityTypeConfiguration<SupplierContact>
{
    public void Configure(EntityTypeBuilder<SupplierContact> builder)
    {
        builder.ToTable("SupplierContacts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.SupplierId).IsRequired();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.JobTitle).HasMaxLength(150);
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.IsActive).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.CreatedBy).HasMaxLength(100);
        builder.Property(c => c.UpdatedBy).HasMaxLength(100);
        builder.Property(c => c.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.Ignore(c => c.FullName);

        builder.HasOne<Supplier>()
               .WithMany()
               .HasForeignKey(c => c.SupplierId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
