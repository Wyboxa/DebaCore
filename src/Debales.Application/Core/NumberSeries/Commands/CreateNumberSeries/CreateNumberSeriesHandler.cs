using Debales.Application.Common;
using Debales.Application.Core.NumberSeries.DTOs;
using Debales.Application.Core.NumberSeries.Queries.GetNumberSeries;
using Series = Debales.Domain.Core.NumberSeries.NumberSeries;

namespace Debales.Application.Core.NumberSeries.Commands.CreateNumberSeries;

public sealed class CreateNumberSeriesHandler
{
    private readonly INumberSeriesRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateNumberSeriesHandler(INumberSeriesRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<NumberSeriesDto> Handle(CreateNumberSeriesCommand command, CancellationToken ct = default)
    {
        var existing = await _repo.GetByCodeAsync(command.Code, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe una serie con el código '{command.Code}'.");

        var series = Series.Create(command.Code, command.Name, command.Prefix, command.Padding, command.CreatedBy);
        await _repo.AddAsync(series, ct);
        await _uow.SaveChangesAsync(ct);
        return GetNumberSeriesHandler.ToDto(series);
    }
}
