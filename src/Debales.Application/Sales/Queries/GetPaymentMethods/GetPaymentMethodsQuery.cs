namespace Debales.Application.Sales.Queries.GetPaymentMethods;

public sealed record GetPaymentMethodsQuery(string? Search = null, bool? IsActive = null, int Page = 1, int PageSize = 50);
