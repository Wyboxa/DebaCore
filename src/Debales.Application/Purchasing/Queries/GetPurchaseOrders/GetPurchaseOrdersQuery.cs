using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseOrders;

public sealed record GetPurchaseOrdersQuery(
    string? Search,
    Guid? SupplierId,
    PurchaseOrderStatus? Status,
    int Page = 1,
    int PageSize = 20);
