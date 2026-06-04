---
type: module
module: compras
layer: cross
status: implemented
source:
  - src/Debales.Domain/Purchasing/
  - src/Debales.Application/Purchasing/
  - src/Debales.Infrastructure/Persistence/Repositories/Purchasing/
  - src/Debales.Api/Controllers/PurchaseOrdersController.cs
  - src/Debales.Api/Controllers/PurchaseDeliveryNotesController.cs
  - src/Debales.Api/Controllers/PurchaseInvoicesController.cs
  - src/Debales.Api/Controllers/PurchaseCreditNotesController.cs
  - src/Debales.Api/Controllers/SupplierPaymentsController.cs
  - src/Debales.Web/Components/Pages/Compras/
related:
  - PurchaseOrder
  - PurchaseOrderLine
  - PurchaseDeliveryNote
  - PurchaseDeliveryNoteLine
  - PurchaseInvoice
  - PurchaseInvoiceLine
  - PurchaseCreditNote
  - PurchaseCreditNoteLine
  - Payable
  - SupplierPayment
  - Supplier
  - Item
---

# Módulo Compras

## Qué problema resuelve

Ciclo completo de compras: desde el pedido al proveedor hasta el pago, pasando por albarán, factura, rectificativa y vencimientos.

## Estado

Implementado — migraciones `AddERP2Module` (2026-05-29) + `AddERP3Module` (2026-06-01).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[PurchaseOrder]] | Pedido de compra al proveedor |
| [[PurchaseOrderLine]] | Línea de pedido de compra |
| [[PurchaseDeliveryNote]] | Albarán de compra vinculado a pedido |
| [[PurchaseDeliveryNoteLine]] | Línea de albarán de compra |
| [[PurchaseInvoice]] | Factura de compra (incluye número de factura del proveedor) |
| [[PurchaseInvoiceLine]] | Línea de factura de compra |
| [[PurchaseCreditNote]] | Rectificativa de compra |
| [[PurchaseCreditNoteLine]] | Línea de rectificativa |
| [[Payable]] | Vencimiento de pago generado desde factura |
| [[SupplierPayment]] | Pago a proveedor que liquida vencimientos |

## Estados

### PurchaseOrder
`Draft → Confirmed → PartiallyReceived → Received | Cancelled`

### PurchaseInvoice
`Draft → Posted | Cancelled`

## Handlers — Commands

| Handler | Descripción |
|---------|-------------|
| `CreatePurchaseOrderHandler` | Crea pedido con líneas |
| `ConfirmPurchaseOrderHandler` | Confirma pedido |
| `CancelPurchaseOrderHandler` | Cancela pedido |
| `CreatePurchaseDeliveryNoteHandler` | Crea albarán de compra |
| `PostPurchaseDeliveryNoteHandler` | Emite albarán |
| `CreatePurchaseInvoiceHandler` | Crea factura de compra |
| `PostPurchaseInvoiceHandler` | Contabiliza factura |
| `CancelPurchaseInvoiceHandler` | Cancela factura Draft |
| `CreatePurchaseCreditNoteHandler` | Crea rectificativa |
| `PostPurchaseCreditNoteHandler` | Contabiliza rectificativa |
| `CreateSupplierPaymentHandler` | Registra pago y liquida vencimientos |

## Handlers — Queries

| Handler | Descripción |
|---------|-------------|
| `GetPurchaseOrdersHandler` | Lista paginada |
| `GetPurchaseOrderByIdHandler` | Pedido con líneas |
| `GetPurchaseDeliveryNotesHandler` | Lista albaranes |
| `GetPurchaseDeliveryNoteByIdHandler` | Albarán con líneas |
| `GetPurchaseInvoicesHandler` | Lista facturas |
| `GetPurchaseInvoiceByIdHandler` | Factura con líneas |
| `GetPurchaseCreditNotesHandler` | Lista rectificativas |
| `GetPurchaseCreditNoteByIdHandler` | Rectificativa con líneas |
| `GetPayablesHandler` | Lista vencimientos |
| `GetSupplierPaymentsHandler` | Lista pagos |

## Controllers

| Controller | Ruta |
|------------|------|
| `PurchaseOrdersController` | `api/purchasing/orders` |
| `PurchaseDeliveryNotesController` | `api/purchasing/delivery-notes` |
| `PurchaseInvoicesController` | `api/purchasing/invoices` |
| `PurchaseCreditNotesController` | `api/purchasing/credit-notes` |
| `SupplierPaymentsController` | `api/supplier-payments` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Pedidos.razor` | `/compras/pedidos` | Implementada |
| `PedidoDetalle.razor` | `/compras/pedidos/{id}` | Implementada |
| `AlbaranesCompra.razor` | `/compras/albaranes` | Implementada |
| `AlbaranCompraDetalle.razor` | `/compras/albaranes/{id}` | Implementada |
| `Compras.razor` | `/compras` | Placeholder de sección |
| `FacturasCompra.razor` | `/facturacion/compras` | Implementada |
| `FacturaCompraDetalle.razor` | `/facturacion/compras/{id}` | Implementada |
| `RectificativasCompra.razor` | `/facturacion/rectificativas-compra` | Implementada |

## Repositorios

- `IPurchaseOrderRepository` → `PurchaseOrderRepository`
- `IPurchaseDeliveryNoteRepository` → `PurchaseDeliveryNoteRepository`
- `IPurchaseInvoiceRepository` → `PurchaseInvoiceRepository`
- `IPurchaseCreditNoteRepository` → `PurchaseCreditNoteRepository`
- `IPayableRepository` → `PayableRepository`
- `ISupplierPaymentRepository` → `SupplierPaymentRepository`

## Lo que está completo

- Ciclo completo: Pedido → Albarán → Factura → Pago
- Rectificativas de compra
- Vencimientos desde factura
- Pagos con liquidación de vencimientos
- Número de factura de proveedor en PurchaseInvoice

## Lo que falta

- Integración automática de pagos con asientos contables
