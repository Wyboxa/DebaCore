using Debales.Domain.CRM.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.CRM;

internal sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("CrmActivities");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CustomerId).IsRequired();
        builder.Property(a => a.Type).IsRequired();
        builder.Property(a => a.Subject).IsRequired().HasMaxLength(300);
        builder.Property(a => a.Notes).HasColumnType("nvarchar(max)");
        builder.Property(a => a.ScheduledAt).IsRequired();
        builder.Property(a => a.AssignedTo).HasMaxLength(100);
        builder.Property(a => a.IsCompleted).IsRequired();
        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(100);
    }
}
