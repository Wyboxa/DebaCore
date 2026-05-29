namespace Debales.Domain.Sales;

public enum SalesOrderStatus
{
    Draft = 0,
    Confirmed = 1,
    PartiallyDelivered = 2,
    Delivered = 3,
    Cancelled = 4
}
