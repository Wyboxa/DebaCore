namespace Debales.Application.Purchasing.Queries.GetSupplierPayments;

public sealed record GetSupplierPaymentsQuery(string? Search, Guid? SupplierId, int Page, int PageSize);
