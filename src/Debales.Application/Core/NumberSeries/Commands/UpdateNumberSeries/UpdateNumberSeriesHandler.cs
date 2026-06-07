using Debales.Application.Common;
using Debales.Application.Core.NumberSeries.DTOs;
using Debales.Application.Core.NumberSeries.Queries.GetNumberSeries;

namespace Debales.Application.Core.NumberSeries.Commands.UpdateNumberSeries;

public sealed class UpdateNumberSeriesHandler
{
    private readonly INumberSeriesRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateNumberSeriesHandler(INumberSeriesRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<NumberSeriesDto> Handle(UpdateNumberSeriesCommand command, CancellationToken ct = default)
    {
        var all = await _repo.GetAllAsync(ct);
        var series = all.FirstOrDefault(s => s.Id == command.Id)
            ?? throw new KeyNotFoundException($"Serie '{command.Id}' no encontrada.");

        series.Update(command.Name, command.Prefix, command.Padding, command.UpdatedBy);

        if (command.NextNumber.HasValue)
            series.SetNextNumber(command.NextNumber.Value, command.UpdatedBy);

        _repo.Update(series);
        await _uow.SaveChangesAsync(ct);
        return GetNumberSeriesHandler.ToDto(series);
    }
}
