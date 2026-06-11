using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Queries.GetRemittances;

public sealed class GetRemittancesHandler(IRemittanceRepository remittances)
{
    public async Task<PagedResult<RemittanceSummaryDto>> Handle(GetRemittancesQuery query, CancellationToken ct = default)
    {
        var result = await remittances.SearchAsync(query.Type, query.Status, query.Page, query.PageSize, ct);
        return new PagedResult<RemittanceSummaryDto>(
            result.Items.Select(ToSummary).ToList(),
            result.TotalCount, result.Page, result.PageSize);
    }

    private static RemittanceSummaryDto ToSummary(Remittance r) => new(
        r.Id, r.Number, r.Date,
        r.BankAccountId, r.BankAccount?.Name ?? "",
        r.Type, r.Type == RemittanceType.Collection ? "Cobro" : "Pago",
        r.Status, StatusLabel(r.Status),
        r.LineCount, r.Total,
        r.CreatedAt, r.CreatedBy);

    private static string StatusLabel(RemittanceStatus s) => s switch
    {
        RemittanceStatus.Draft     => "Borrador",
        RemittanceStatus.Sent      => "Enviada",
        RemittanceStatus.Confirmed => "Confirmada",
        RemittanceStatus.Failed    => "Fallida",
        _                          => s.ToString()
    };
}
