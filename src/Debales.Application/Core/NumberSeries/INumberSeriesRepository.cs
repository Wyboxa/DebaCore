using Series = Debales.Domain.Core.NumberSeries.NumberSeries;

namespace Debales.Application.Core.NumberSeries;

public interface INumberSeriesRepository
{
    Task<IReadOnlyList<Series>> GetAllAsync(CancellationToken ct = default);
    Task<Series?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(Series series, CancellationToken ct = default);
    void Update(Series series);
}
