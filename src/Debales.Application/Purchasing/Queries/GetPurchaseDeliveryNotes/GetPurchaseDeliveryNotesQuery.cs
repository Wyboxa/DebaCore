using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNotes;

public sealed record GetPurchaseDeliveryNotesQuery(
    string? Search,
    Guid? SupplierId,
    PurchaseDeliveryNoteStatus? Status,
    int Page = 1,
    int PageSize = 20);
