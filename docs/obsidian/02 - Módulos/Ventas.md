---
type: module
module: ventas
layer: cross
status: implemented
source:
  - src/Debales.Domain/Sales/
  - src/Debales.Application/Sales/
  - src/Debales.Infrastructure/Persistence/Repositories/Sales/
  - src/Debales.Api/Controllers/SalesOrdersController.cs
  - src/Debales.Api/Controllers/SalesDeliveryNotesController.cs
  - src/Debales.Api/Controllers/SalesInvoicesController.cs
  - src/Debales.Api/Controllers/SalesCreditNotesController.cs
  - src/Debales.Api/Controllers/CustomerPaymentsController.cs
  - src/Debales.Web/Components/Pages/Ventas/
related:
  - SalesOrder
  - SalesOrderLine
  - SalesDeliveryNote
  - SalesDeliveryNoteLine
  - SalesInvoice
  - SalesInvoiceLine
  - SalesCreditNote
  - SalesCreditNoteLine
  - Receivable
  - CustomerPayment
  - Customer
  - Item
---

# Módulo Ventas

## Qué problema resuelve

Ciclo completo de ventas: desde el pedido del cliente hasta el cobro, pasando por albarán, factura, rectificativa y vencimientos.

## Estado

Implementado — migraciones `AddERP2Module` (2026-05-29) + `AddERP3Module` (2026-06-01).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[SalesOrder]] | Pedido de venta con líneas, estados y cálculos de totales |
| [[SalesOrderLine]] | Línea de pedido con artículo, cantidades entregadas y pendientes |
| [[SalesDeliveryNote]] | Albarán de venta vinculado a pedido, con líneas |
| [[SalesDeliveryNoteLine]] | Línea de albarán |
| [[SalesInvoice]] | Factura de venta vinculada a albarán, con ciclo Draft/Posted/Cancelled |
| [[SalesInvoiceLine]] | Línea de factura |
| [[SalesCreditNote]] | Factura rectificativa de venta |
| [[SalesCreditNoteLine]] | Línea de rectificativa |
| [[Receivable]] | Vencimiento de cobro generado desde factura |
| [[CustomerPayment]] | Cobro de cliente que liquida vencimientos |

## Estados

### SalesOrder
`Draft → Confirmed → PartiallyDelivered → Delivered | Cancelled`

### SalesInvoice
`Draft → Posted | Cancelled`

### SalesDeliveryNote
`Draft → Posted`

## Handlers — Commands

| Handler | Descripción |
|---------|-------------|
| `CreateSalesOrderHandler` | Crea pedido con líneas |
| `ConfirmSalesOrderHandler` | Confirma pedido Draft |
| `CancelSalesOrderHandler` | Cancela pedido |
| `CreateSalesDeliveryNoteHandler` | Crea albarán manual |
| `PostSalesDeliveryNoteHandler` | Emite albarán |
| `GenerateDeliveryNoteFromOrderHandler` | Genera albarán automático desde pedido confirmado |
| `GenerateInvoiceFromDeliveryNoteHandler` | Genera factura automática desde albarán emitido |
| `BatchGenerateDocumentsHandler` | Generación en lote (pedido → albarán → factura) |
| `CreateSalesInvoiceHandler` | Crea factura manual |
| `PostSalesInvoiceHandler` | Contabiliza factura |
| `CancelSalesInvoiceHandler` | Cancela factura Draft |
| `CreateSalesCreditNoteHandler` | Crea rectificativa |
| `PostSalesCreditNoteHandler` | Contabiliza rectificativa |
| `CreateCustomerPaymentHandler` | Registra cobro y liquida vencimientos |

## Handlers — Queries

| Handler | Descripción |
|---------|-------------|
| `GetSalesOrdersHandler` | Lista paginada con filtros |
| `GetSalesOrderByIdHandler` | Pedido con líneas |
| `GetSalesDeliveryNotesHandler` | Lista albaranes |
| `GetSalesDeliveryNoteByIdHandler` | Albarán con líneas |
| `GetSalesInvoicesHandler` | Lista facturas |
| `GetSalesInvoiceByIdHandler` | Factura con líneas |
| `GetSalesCreditNotesHandler` | Lista rectificativas |
| `GetSalesCreditNoteByIdHandler` | Rectificativa con líneas |
| `GetReceivablesHandler` | Lista vencimientos |
| `GetCustomerPaymentsHandler` | Lista cobros |
| `GetAutomationPreviewHandler` | Preview de automatización pedido→albarán→factura |

## Controllers

| Controller | Ruta |
|------------|------|
| `SalesOrdersController` | `api/sales/orders` |
| `SalesDeliveryNotesController` | `api/sales/delivery-notes` |
| `SalesInvoicesController` | `api/sales/invoices` |
| `SalesCreditNotesController` | `api/sales/credit-notes` |
| `CustomerPaymentsController` | `api/customers/payments` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Pedidos.razor` | `/ventas/pedidos` | Implementada — lista + modal creación con líneas |
| `PedidoDetalle.razor` | `/ventas/pedidos/{id}` | Implementada — ficha pedido |
| `AlbaranesVenta.razor` | `/ventas/albaranes` | Implementada |
| `AlbaranVentaDetalle.razor` | `/ventas/albaranes/{id}` | Implementada |
| `Automatizacion.razor` | `/ventas/automatizacion` | Implementada — generación en lote |
| `Ventas.razor` | `/ventas` | Placeholder de sección |
| `FacturasVenta.razor` | `/facturacion/ventas` | Implementada |
| `FacturaVentaDetalle.razor` | `/facturacion/ventas/{id}` | Implementada |
| `RectificativasVenta.razor` | `/facturacion/rectificativas-venta` | Implementada |

## Repositorios

- `ISalesOrderRepository` → `SalesOrderRepository`
- `ISalesDeliveryNoteRepository` → `SalesDeliveryNoteRepository`
- `ISalesInvoiceRepository` → `SalesInvoiceRepository`
- `ISalesCreditNoteRepository` → `SalesCreditNoteRepository`
- `IReceivableRepository` → `ReceivableRepository`
- `ICustomerPaymentRepository` → `CustomerPaymentRepository`

## Lo que está completo

- Ciclo completo: Pedido → Albarán → Factura → Cobro
- Rectificativas de venta
- Generación automática en lote
- Vencimientos desde factura
- Cobros con liquidación de vencimientos
- UI completa con modales de creación

## Lo que falta

- Presupuestos de venta (`SalesQuote`)
- Integración automática de cobros con asientos contables
