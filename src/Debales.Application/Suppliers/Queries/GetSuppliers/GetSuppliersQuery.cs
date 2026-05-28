namespace Debales.Application.Suppliers.Queries.GetSuppliers;

public sealed record GetSuppliersQuery(string? Search, int Page = 1, int PageSize = 20);
