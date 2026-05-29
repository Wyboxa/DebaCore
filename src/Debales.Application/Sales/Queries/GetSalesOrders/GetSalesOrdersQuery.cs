using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesOrders;

public sealed record GetSalesOrdersQuery(
    string? Search,
    Guid? CustomerId,
    SalesOrderStatus? Status,
    int Page = 1,
    int PageSize = 20);
