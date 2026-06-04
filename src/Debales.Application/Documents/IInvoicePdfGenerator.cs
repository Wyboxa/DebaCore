using Debales.Application.Sales.DTOs;
using Debales.Application.Purchasing.DTOs;

namespace Debales.Application.Documents;

public interface IInvoicePdfGenerator
{
    byte[] GenerateSalesInvoice(SalesInvoiceDetailDto invoice);
    byte[] GeneratePurchaseInvoice(PurchaseInvoiceDetailDto invoice);
}
