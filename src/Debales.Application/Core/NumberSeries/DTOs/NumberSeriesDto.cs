namespace Debales.Application.Core.NumberSeries.DTOs;

public sealed record NumberSeriesDto(
    Guid Id,
    string Code,
    string Name,
    string Prefix,
    int Padding,
    int NextNumber,
    bool IsActive,
    string Preview);
