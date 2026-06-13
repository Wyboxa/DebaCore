using Debales.Domain.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Documents;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title).IsRequired().HasMaxLength(255);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.DocumentTypeId).IsRequired();
        builder.Property(d => d.FileName).HasMaxLength(255);
        builder.Property(d => d.MimeType).HasMaxLength(100);
        builder.Property(d => d.Notes).HasMaxLength(2000);
        builder.Property(d => d.DocumentDate).IsRequired();
        builder.Property(d => d.IsActive).IsRequired();
        builder.Property(d => d.CreatedAt).IsRequired();
        builder.Property(d => d.CreatedBy).HasMaxLength(100);
        builder.Property(d => d.UpdatedBy).HasMaxLength(100);
        builder.Property(d => d.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(d => !d.IsDeleted);

        builder.HasOne<DocumentType>()
               .WithMany()
               .HasForeignKey(d => d.DocumentTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => d.DocumentTypeId);
        builder.HasIndex(d => d.CustomerId);
        builder.HasIndex(d => d.SupplierId);
    }
}
