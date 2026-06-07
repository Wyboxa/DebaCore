using Debales.Application.Core.NumberSeries.DTOs;
using Series = Debales.Domain.Core.NumberSeries.NumberSeries;

namespace Debales.Application.Core.NumberSeries.Queries.GetNumberSeries;

public sealed class GetNumberSeriesHandler
{
    private readonly INumberSeriesRepository _repo;

    public GetNumberSeriesHandler(INumberSeriesRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<NumberSeriesDto>> Handle(CancellationToken ct = default)
    {
        var all = await _repo.GetAllAsync(ct);
        return all.Select(ToDto).ToList();
    }

    internal static NumberSeriesDto ToDto(Series s) =>
        new(s.Id, s.Code, s.Name, s.Prefix, s.Padding, s.NextNumber, s.IsActive,
            Preview: s.GetNextFormatted());
}
