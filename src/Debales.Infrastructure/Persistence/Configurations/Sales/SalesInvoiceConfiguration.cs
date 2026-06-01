using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> builder)
    {
        builder.ToTable("SalesInvoices");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Number).HasMaxLength(30).IsRequired();
        builder.Property(i => i.Status).IsRequired();
        builder.Property(i => i.Notes).HasMaxLength(1000);

        builder.HasIndex(i => i.Number).IsUnique();
        builder.HasIndex(i => i.CustomerId);
        builder.HasIndex(i => i.Status);

        builder.HasOne(i => i.Customer)
            .WithMany()
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.SalesDeliveryNote)
            .WithMany()
            .HasForeignKey(i => i.SalesDeliveryNoteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(i => i.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
