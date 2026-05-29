using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesDeliveryNotes;

public sealed record GetSalesDeliveryNotesQuery(
    string? Search,
    Guid? CustomerId,
    SalesDeliveryNoteStatus? Status,
    int Page = 1,
    int PageSize = 20);
