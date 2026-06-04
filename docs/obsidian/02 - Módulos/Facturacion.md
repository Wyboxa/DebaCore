---
type: module
module: ventas
layer: cross
status: implemented
source:
  - src/Debales.Domain/Sales/SalesInvoice.cs
  - src/Debales.Domain/Sales/Receivable.cs
  - src/Debales.Domain/Purchasing/PurchaseInvoice.cs
  - src/Debales.Domain/Purchasing/Payable.cs
  - src/Debales.Web/Components/Pages/Facturacion/
related:
  - SalesInvoice
  - SalesCreditNote
  - Receivable
  - CustomerPayment
  - PurchaseInvoice
  - PurchaseCreditNote
  - Payable
  - SupplierPayment
  - Ventas
  - Compras
---

# Módulo Facturación

## Qué problema resuelve

Agrupación funcional de las facturas, rectificativas, vencimientos y cobros/pagos de ventas y compras. No tiene entidades propias — es una vista cross de Ventas y Compras.

## Estado

Implementado — migraciones `AddERP3Module` (2026-06-01).

## Entidades de facturación de venta

| Entidad | Descripción |
|---------|-------------|
| [[SalesInvoice]] | Factura de venta — ciclo Draft → Posted → Cancelled |
| [[SalesCreditNote]] | Rectificativa de venta |
| [[Receivable]] | Vencimiento de cobro (Pending / Partial / Settled) |
| [[CustomerPayment]] | Cobro de cliente |

## Entidades de facturación de compra

| Entidad | Descripción |
|---------|-------------|
| [[PurchaseInvoice]] | Factura de compra — ciclo Draft → Posted → Cancelled |
| [[PurchaseCreditNote]] | Rectificativa de compra |
| [[Payable]] | Vencimiento de pago (Pending / Partial / Settled) |
| [[SupplierPayment]] | Pago a proveedor |

## Reglas de negocio clave

- Una factura Posted no puede cancelarse directamente — hay que emitir rectificativa
- `DueDate >= Date` (validado en `SalesInvoice.Create` y `PurchaseInvoice.Create`)
- Los vencimientos se generan al contabilizar la factura
- Un cobro/pago puede liquidar parcialmente un vencimiento

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Facturacion.razor` | `/facturacion` | Placeholder de sección |
| `FacturasVenta.razor` | `/facturacion/ventas` | Implementada |
| `FacturaVentaDetalle.razor` | `/facturacion/ventas/{id}` | Implementada |
| `RectificativasVenta.razor` | `/facturacion/rectificativas-venta` | Implementada |
| `RectificativaVentaDetalle.razor` | `/facturacion/rectificativas-venta/{id}` | Implementada |
| `FacturasCompra.razor` | `/facturacion/compras` | Implementada |
| `FacturaCompraDetalle.razor` | `/facturacion/compras/{id}` | Implementada |
| `RectificativasCompra.razor` | `/facturacion/rectificativas-compra` | Implementada |
| `RectificativaCompraDetalle.razor` | `/facturacion/rectificativas-compra/{id}` | Implementada |

## Lo que falta

- Series de facturación configurables (`InvoiceSeries`)
- Condiciones de pago (`PaymentTerm`)
- Formas de pago (`PaymentMethod`)
- Generación de PDF de factura
- Envío por email
