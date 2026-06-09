namespace Debales.Application.Sales.Queries.GetPaymentTerms;

public sealed record GetPaymentTermsQuery(string? Search = null, bool? IsActive = null, int Page = 1, int PageSize = 50);
