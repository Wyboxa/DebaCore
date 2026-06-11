using Debales.Application.Accounting.Queries.GetTreasuryPosition;
using Debales.Application.Purchasing.Queries.GetPayablesAging;
using Debales.Application.Purchasing.Queries.GetSupplierStatement;
using Debales.Application.Sales.Queries.GetCustomerStatement;
using Debales.Application.Sales.Queries.GetReceivablesAging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ReportsController : ControllerBase
{
    private readonly GetReceivablesAgingHandler _receivablesAging;
    private readonly GetPayablesAgingHandler _payablesAging;
    private readonly GetTreasuryPositionHandler _treasury;
    private readonly GetCustomerStatementHandler _customerStatement;
    private readonly GetSupplierStatementHandler _supplierStatement;

    public ReportsController(
        GetReceivablesAgingHandler receivablesAging,
        GetPayablesAgingHandler payablesAging,
        GetTreasuryPositionHandler treasury,
        GetCustomerStatementHandler customerStatement,
        GetSupplierStatementHandler supplierStatement)
    {
        _receivablesAging = receivablesAging;
        _payablesAging = payablesAging;
        _treasury = treasury;
        _customerStatement = customerStatement;
        _supplierStatement = supplierStatement;
    }

    [HttpGet("receivables-aging")]
    public async Task<IActionResult> ReceivablesAging([FromQuery] Guid? customerId)
        => Ok(await _receivablesAging.Handle(new GetReceivablesAgingQuery(customerId)));

    [HttpGet("payables-aging")]
    public async Task<IActionResult> PayablesAging([FromQuery] Guid? supplierId)
        => Ok(await _payablesAging.Handle(new GetPayablesAgingQuery(supplierId)));

    [HttpGet("treasury-position")]
    public async Task<IActionResult> TreasuryPosition()
        => Ok(await _treasury.Handle(new GetTreasuryPositionQuery()));

    [HttpGet("customer-statement/{customerId:guid}")]
    public async Task<IActionResult> CustomerStatement(
        Guid customerId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
        => Ok(await _customerStatement.Handle(new GetCustomerStatementQuery(customerId, from, to)));

    [HttpGet("supplier-statement/{supplierId:guid}")]
    public async Task<IActionResult> SupplierStatement(
        Guid supplierId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
        => Ok(await _supplierStatement.Handle(new GetSupplierStatementQuery(supplierId, from, to)));
}
