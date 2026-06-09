using Debales.Application.Core.NumberSeries.Commands.CreateNumberSeries;
using Debales.Application.Core.NumberSeries.Commands.UpdateNumberSeries;
using Debales.Application.Core.NumberSeries.Queries.GetNumberSeries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class NumberSeriesController : ControllerBase
{
    private readonly GetNumberSeriesHandler _get;
    private readonly CreateNumberSeriesHandler _create;
    private readonly UpdateNumberSeriesHandler _update;

    public NumberSeriesController(
        GetNumberSeriesHandler get,
        CreateNumberSeriesHandler create,
        UpdateNumberSeriesHandler update)
    {
        _get = get;
        _create = create;
        _update = update;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default) =>
        Ok(await _get.Handle(ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNumberSeriesRequest request, CancellationToken ct = default)
    {
        var dto = await _create.Handle(
            new CreateNumberSeriesCommand(request.Code, request.Name, request.Prefix, request.Padding, "api"), ct);
        return CreatedAtAction(nameof(GetAll), new { }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNumberSeriesRequest request, CancellationToken ct = default)
    {
        var dto = await _update.Handle(
            new UpdateNumberSeriesCommand(id, request.Name, request.Prefix, request.Padding, request.NextNumber, "api"), ct);
        return Ok(dto);
    }

    public sealed record CreateNumberSeriesRequest(string Code, string Name, string Prefix, int Padding);
    public sealed record UpdateNumberSeriesRequest(string Name, string Prefix, int Padding, int? NextNumber);
}
