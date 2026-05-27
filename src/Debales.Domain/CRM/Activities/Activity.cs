using Debales.Domain.Common;

namespace Debales.Domain.CRM.Activities;

public sealed class Activity : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid? ContactId { get; private set; }
    public ActivityType Type { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public bool IsCompleted { get; private set; }
    public string? AssignedTo { get; private set; }

    private Activity() { }

    public static Activity Log(
        Guid customerId,
        ActivityType type,
        string subject,
        DateTime scheduledAt,
        string createdBy,
        Guid? contactId = null,
        string? notes = null,
        string? assignedTo = null)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("El asunto no puede estar vacío.", nameof(subject));

        return new Activity
        {
            CustomerId = customerId,
            ContactId = contactId,
            Type = type,
            Subject = subject.Trim(),
            Notes = notes?.Trim(),
            ScheduledAt = scheduledAt,
            AssignedTo = assignedTo?.Trim(),
            IsCompleted = false,
            CreatedBy = createdBy
        };
    }

    public void Complete(string? notes = null)
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        if (notes is not null)
            Notes = notes.Trim();
    }
}
