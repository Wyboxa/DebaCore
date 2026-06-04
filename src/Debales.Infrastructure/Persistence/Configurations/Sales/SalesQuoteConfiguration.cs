using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesQuoteConfiguration : IEntityTypeConfiguration<SalesQuote>
{
    public void Configure(EntityTypeBuilder<SalesQuote> builder)
    {
        builder.ToTable("SalesQuotes");
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Number).HasMaxLength(30).IsRequired();
        builder.Property(q => q.Status).IsRequired();
        builder.Property(q => q.Notes).HasMaxLength(1000);

        builder.HasIndex(q => q.Number).IsUnique();
        builder.HasIndex(q => q.CustomerId);
        builder.HasIndex(q => q.Status);

        builder.HasOne(q => q.Customer)
            .WithMany()
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesQuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(q => !q.IsDeleted);
    }
}
