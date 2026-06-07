namespace Debales.Application.Core.NumberSeries.Commands.UpdateNumberSeries;

public sealed record UpdateNumberSeriesCommand(
    Guid Id,
    string Name,
    string Prefix,
    int Padding,
    int? NextNumber,
    string UpdatedBy);
