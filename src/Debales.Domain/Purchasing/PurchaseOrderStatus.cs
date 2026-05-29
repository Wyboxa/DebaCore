namespace Debales.Domain.Purchasing;

public enum PurchaseOrderStatus
{
    Draft = 0,
    Confirmed = 1,
    PartiallyReceived = 2,
    Received = 3,
    Cancelled = 4
}
