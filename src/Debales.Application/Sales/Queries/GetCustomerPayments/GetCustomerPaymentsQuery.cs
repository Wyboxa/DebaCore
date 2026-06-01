namespace Debales.Application.Sales.Queries.GetCustomerPayments;

public sealed record GetCustomerPaymentsQuery(string? Search, Guid? CustomerId, int Page, int PageSize);
