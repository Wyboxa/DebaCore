using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesDeliveryNoteConfiguration : IEntityTypeConfiguration<SalesDeliveryNote>
{
    public void Configure(EntityTypeBuilder<SalesDeliveryNote> builder)
    {
        builder.ToTable("SalesDeliveryNotes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Number).HasMaxLength(30).IsRequired();
        builder.Property(n => n.Status).IsRequired();
        builder.Property(n => n.Notes).HasMaxLength(1000);

        builder.HasIndex(n => n.Number).IsUnique();
        builder.HasIndex(n => n.CustomerId);
        builder.HasIndex(n => n.SalesOrderId);

        builder.HasOne(n => n.Customer)
            .WithMany()
            .HasForeignKey(n => n.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.SalesOrder)
            .WithMany()
            .HasForeignKey(n => n.SalesOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesDeliveryNoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
