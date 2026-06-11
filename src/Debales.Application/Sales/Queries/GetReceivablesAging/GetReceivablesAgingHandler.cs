using Debales.Application.Common;

namespace Debales.Application.Sales.Queries.GetReceivablesAging;

public sealed class GetReceivablesAgingHandler(IReceivableRepository receivables)
{
    public async Task<AgingReportDto> Handle(GetReceivablesAgingQuery query, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var items = await receivables.GetForAgingAsync(query.CustomerId, ct);

        var agingItems = items.Select(r => new AgingItemDto(
            r.Id,
            r.Number,
            r.CustomerId,
            r.Customer?.Name ?? "",
            r.DueDate,
            r.OriginalAmount,
            r.PendingAmount,
            Math.Max(0, today.DayNumber - r.DueDate.DayNumber)
        )).ToList();

        var current   = Bucket(agingItems, i => i.DaysOverdue == 0);
        var b1_30     = Bucket(agingItems, i => i.DaysOverdue is >= 1 and <= 30);
        var b31_60    = Bucket(agingItems, i => i.DaysOverdue is >= 31 and <= 60);
        var b61_90    = Bucket(agingItems, i => i.DaysOverdue is >= 61 and <= 90);
        var bOver90   = Bucket(agingItems, i => i.DaysOverdue > 90);

        var totalPending = agingItems.Sum(i => i.PendingAmount);
        var totalOverdue = agingItems.Where(i => i.DaysOverdue > 0).Sum(i => i.PendingAmount);

        return new AgingReportDto(today, totalPending, totalOverdue,
            new AgingBucketDto("Al corriente",    current.Count,  current.Amount),
            new AgingBucketDto("1-30 días",       b1_30.Count,    b1_30.Amount),
            new AgingBucketDto("31-60 días",      b31_60.Count,   b31_60.Amount),
            new AgingBucketDto("61-90 días",      b61_90.Count,   b61_90.Amount),
            new AgingBucketDto("Más de 90 días",  bOver90.Count,  bOver90.Amount),
            agingItems);
    }

    private static (int Count, decimal Amount) Bucket(
        IEnumerable<AgingItemDto> items, Func<AgingItemDto, bool> predicate)
    {
        var filtered = items.Where(predicate).ToList();
        return (filtered.Count, filtered.Sum(i => i.PendingAmount));
    }
}
