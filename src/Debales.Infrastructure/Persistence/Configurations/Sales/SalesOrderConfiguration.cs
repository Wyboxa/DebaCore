using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Number).HasMaxLength(30).IsRequired();
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.Notes).HasMaxLength(1000);

        builder.HasIndex(o => o.Number).IsUnique();
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);

        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
