using Debales.Domain.Common;

namespace Debales.Domain.CRM.Opportunities;

public sealed class Opportunity : AuditableEntity
{
    public Guid CustomerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal? EstimatedValue { get; private set; }
    public OpportunityStatus Status { get; private set; }
    public DateTime? ExpectedCloseDate { get; private set; }

    private Opportunity() { }

    public static Opportunity Create(
        Guid customerId,
        string title,
        string createdBy,
        decimal? estimatedValue = null,
        DateTime? expectedCloseDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío.", nameof(title));

        return new Opportunity
        {
            CustomerId = customerId,
            Title = title.Trim(),
            EstimatedValue = estimatedValue,
            Status = OpportunityStatus.New,
            ExpectedCloseDate = expectedCloseDate,
            CreatedBy = createdBy
        };
    }

    public void UpdateStatus(OpportunityStatus newStatus, string updatedBy)
    {
        Status = newStatus;
        SetUpdated(updatedBy);
    }
}
