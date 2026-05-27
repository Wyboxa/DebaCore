namespace Debales.Application.CRM.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery(string? Search = null, int Page = 1, int PageSize = 20);
