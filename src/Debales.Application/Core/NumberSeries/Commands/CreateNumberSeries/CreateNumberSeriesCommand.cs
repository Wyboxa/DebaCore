namespace Debales.Application.Core.NumberSeries.Commands.CreateNumberSeries;

public sealed record CreateNumberSeriesCommand(
    string Code,
    string Name,
    string Prefix,
    int Padding,
    string CreatedBy);
