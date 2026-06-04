---
type: entity
module: ventas
layer: domain
status: implemented
source:
  - src/Debales.Domain/Sales/SalesInvoice.cs
  - src/Debales.Domain/Sales/SalesInvoiceLine.cs
  - src/Debales.Domain/Sales/SalesInvoiceStatus.cs
related:
  - Ventas
  - Facturacion
  - Customer
  - SalesDeliveryNote
  - Receivable
  - AccountingEntry
---

# SalesInvoice (Factura de Venta)

## Tabla EF / DbSet

`SalesInvoices` — `DbSet<SalesInvoice>`
`SalesInvoiceLines` — `DbSet<SalesInvoiceLine>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Number` | `string` | Número de factura (uppercase) |
| `CustomerId` | `Guid` | FK a Customer |
| `SalesDeliveryNoteId` | `Guid?` | FK a SalesDeliveryNote (opcional) |
| `Date` | `DateOnly` | Fecha de factura |
| `DueDate` | `DateOnly` | Fecha de vencimiento (≥ Date) |
| `Status` | `SalesInvoiceStatus` | Estado |
| `Notes` | `string?` | Observaciones |

## Estados (SalesInvoiceStatus)

`Draft → Posted | Cancelled`

**Regla crítica**: Una factura `Posted` no puede cancelarse directamente. Debe emitirse una [[SalesCreditNote]].

## Propiedades calculadas

| Propiedad | Descripción |
|-----------|-------------|
| `Subtotal` | Suma base imponible de líneas |
| `TaxAmount` | Suma IVA de líneas |
| `Total` | Suma total de líneas |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| Customer (FK CustomerId) | Explícita |
| SalesDeliveryNote (FK SalesDeliveryNoteId) | Explícita |
| SalesInvoiceLine | Explícita 1:N |
| Receivable (generado al Post — inferido via handler) | Inferida |
| AccountingEntry (SourceType="SalesInvoice", SourceId=Id) | Inferida |

## Handlers que usan esta entidad

- `CreateSalesInvoiceHandler`, `PostSalesInvoiceHandler`, `CancelSalesInvoiceHandler`
- `GenerateInvoiceFromDeliveryNoteHandler` — crea factura desde albarán
