using Debales.Application.Common;
using Debales.Application.Sales.Commands.CreateCustomerPayment;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Queries.GetCustomerPayments;

public sealed class GetCustomerPaymentsHandler
{
    private readonly ICustomerPaymentRepository _payments;

    public GetCustomerPaymentsHandler(ICustomerPaymentRepository payments) => _payments = payments;

    public async Task<PagedResult<CustomerPaymentSummaryDto>> Handle(GetCustomerPaymentsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _payments.SearchAsync(query.Search, query.CustomerId, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(p => new CustomerPaymentSummaryDto(
            p.Id, p.Number,
            p.CustomerId, p.Customer?.Name ?? string.Empty,
            p.ReceivableId, p.Receivable?.Number,
            p.Date, p.Amount, p.Reference)).ToList();
        return new PagedResult<CustomerPaymentSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
