namespace Debales.Application.Core.Audit.Queries.GetAuditEntries;

public sealed record GetAuditEntriesQuery(
    string? EntityName = null,
    DateTime? From = null,
    DateTime? To = null,
    int Take = 200);
