using Debales.Domain.CRM.Notes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.CRM;

internal sealed class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("CrmNotes");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.CustomerId).IsRequired();
        builder.Property(n => n.Content).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.CreatedBy).HasMaxLength(100);
    }
}
