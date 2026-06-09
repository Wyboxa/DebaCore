using Debales.Domain.Common;

namespace Debales.Domain.Sales;

public sealed class PaymentTermLine : Entity
{
    private PaymentTermLine() { }

    public Guid PaymentTermId { get; private set; }
    public int DueDays { get; private set; }
    public decimal Percentage { get; private set; }
    public int SortOrder { get; private set; }

    public PaymentTerm? PaymentTerm { get; private set; }

    internal static PaymentTermLine Create(Guid paymentTermId, int dueDays, decimal percentage, int sortOrder)
        => new() { PaymentTermId = paymentTermId, DueDays = dueDays, Percentage = percentage, SortOrder = sortOrder };
}
