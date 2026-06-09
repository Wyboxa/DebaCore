using Debales.Application.Purchasing.Commands.CreateSupplierPayment;
using Debales.Application.Purchasing.Queries.GetPayables;
using Debales.Application.Purchasing.Queries.GetSupplierPayments;
using Debales.Domain.Purchasing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/purchasing")]
[Authorize]
[RequiresModule("Purchasing")]
public sealed class SupplierPaymentsController : ControllerBase
{
    private readonly GetPayablesHandler _getPayables;
    private readonly CreateSupplierPaymentHandler _createPayment;
    private readonly GetSupplierPaymentsHandler _getPayments;

    public SupplierPaymentsController(
        GetPayablesHandler getPayables,
        CreateSupplierPaymentHandler createPayment,
        GetSupplierPaymentsHandler getPayments)
    {
        _getPayables = getPayables;
        _createPayment = createPayment;
        _getPayments = getPayments;
    }

    [HttpGet("payables")]
    public async Task<IActionResult> GetPayables(
        [FromQuery] string? search, [FromQuery] Guid? supplierId,
        [FromQuery] PayableStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getPayables.Handle(new GetPayablesQuery(search, supplierId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments(
        [FromQuery] string? search, [FromQuery] Guid? supplierId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getPayments.Handle(new GetSupplierPaymentsQuery(search, supplierId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpPost("payments")]
    public async Task<IActionResult> CreatePayment([FromBody] SupplierPaymentBody request, CancellationToken ct = default)
    {
        var payment = await _createPayment.Handle(
            new CreateSupplierPaymentCommand(
                request.SupplierId, request.PayableId,
                request.Date, request.Amount, request.Reference, request.Notes, "api"), ct);
        return Ok(payment);
    }

    public sealed record SupplierPaymentBody(
        Guid SupplierId, Guid? PayableId,
        DateOnly Date, decimal Amount, string? Reference, string? Notes);
}
