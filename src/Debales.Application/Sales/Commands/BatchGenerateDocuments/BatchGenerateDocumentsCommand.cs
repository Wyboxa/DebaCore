namespace Debales.Application.Sales.Commands.BatchGenerateDocuments;

public enum BatchMode
{
    OrdersToDeliveryNotes,
    DeliveryNotesToInvoices
}

public sealed record BatchGenerateDocumentsCommand(
    BatchMode Mode,
    string CreatedBy);

public sealed record BatchItemResult(
    Guid SourceId,
    string SourceNumber,
    string? GeneratedNumber,
    bool Success,
    string? Error);

public sealed record BatchGenerateDocumentsResult(
    BatchMode Mode,
    int TotalFound,
    int Succeeded,
    int Failed,
    IReadOnlyList<BatchItemResult> Items);
