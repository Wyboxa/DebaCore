using Debales.Application.Sales.Commands.CreateCustomerPayment;
using Debales.Application.Sales.Queries.GetCustomerPayments;
using Debales.Application.Sales.Queries.GetReceivables;
using Debales.Domain.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/sales")]
[Authorize]
public sealed class CustomerPaymentsController : ControllerBase
{
    private readonly GetReceivablesHandler _getReceivables;
    private readonly CreateCustomerPaymentHandler _createPayment;
    private readonly GetCustomerPaymentsHandler _getPayments;

    public CustomerPaymentsController(
        GetReceivablesHandler getReceivables,
        CreateCustomerPaymentHandler createPayment,
        GetCustomerPaymentsHandler getPayments)
    {
        _getReceivables = getReceivables;
        _createPayment = createPayment;
        _getPayments = getPayments;
    }

    [HttpGet("receivables")]
    public async Task<IActionResult> GetReceivables(
        [FromQuery] string? search, [FromQuery] Guid? customerId,
        [FromQuery] ReceivableStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getReceivables.Handle(new GetReceivablesQuery(search, customerId, status, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments(
        [FromQuery] string? search, [FromQuery] Guid? customerId,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getPayments.Handle(new GetCustomerPaymentsQuery(search, customerId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpPost("payments")]
    public async Task<IActionResult> CreatePayment([FromBody] CustomerPaymentBody request, CancellationToken ct = default)
    {
        var payment = await _createPayment.Handle(
            new CreateCustomerPaymentCommand(
                request.CustomerId, request.ReceivableId,
                request.Date, request.Amount, request.Reference, request.Notes, "api"), ct);
        return Ok(payment);
    }

    public sealed record CustomerPaymentBody(
        Guid CustomerId, Guid? ReceivableId,
        DateOnly Date, decimal Amount, string? Reference, string? Notes);
}
