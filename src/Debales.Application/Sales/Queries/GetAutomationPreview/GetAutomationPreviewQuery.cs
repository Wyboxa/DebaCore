namespace Debales.Application.Sales.Queries.GetAutomationPreview;

public sealed record AutomationPreviewItem(
    Guid Id,
    string Number,
    string CustomerName,
    DateOnly Date,
    string StatusLabel);

public sealed record AutomationPreviewResult(
    IReadOnlyList<AutomationPreviewItem> PendingOrdersForDelivery,
    IReadOnlyList<AutomationPreviewItem> PostedNotesForInvoice);
