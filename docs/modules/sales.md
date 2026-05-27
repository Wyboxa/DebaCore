# Módulo Sales — Debales

## Estado

Pendiente de implementar (Fase ERP-2 / ERP-3)

## Dependencias

- Core (usuarios, series, auditoría)
- CRM (clientes)
- Catalog (artículos, tarifas, IVA)

## Entidades

```txt
SalesQuote
 ├── CustomerId
 ├── Date, ValidUntil
 ├── Status (Draft | Sent | Accepted | Rejected | Expired)
 ├── Lines: SalesQuoteLine[]
 │     ├── ItemId / ServiceId
 │     ├── Description, Quantity, UnitPrice, Discount, TaxTypeId
 │     └── LineTotal (calculado)
 └── TotalNet, TotalTax, TotalGross

SalesOrder
 ├── CustomerId, QuoteId? (si viene de presupuesto)
 ├── NumberSeriesId, Number
 ├── Date, DeliveryDate?
 ├── Status (Open | PartiallyDelivered | Delivered | Cancelled)
 ├── Lines: SalesOrderLine[]
 └── Totals

SalesDeliveryNote
 ├── CustomerId, OrderId?
 ├── NumberSeriesId, Number
 ├── Date
 ├── Status (Draft | Posted | Cancelled)
 ├── Lines: SalesDeliveryNoteLine[]
 │     ├── OrderLineId?
 │     ├── ItemId, Quantity, WarehouseId
 │     └── StockMovementId? (si Inventory activo)
 └── Totals

SalesInvoice
 ├── CustomerId, DeliveryNoteId?
 ├── NumberSeriesId, Number
 ├── Date, DueDate
 ├── Status (Draft | Posted | Cancelled)
 ├── Lines: SalesInvoiceLine[]
 ├── Totals
 ├── AccountingEntryId? (si Accounting activo)
 └── Receivables: Receivable[]

SalesCreditNote
 ├── OriginalInvoiceId
 ├── NumberSeriesId, Number
 ├── Date, Reason
 ├── Status (Draft | Posted | Cancelled)
 └── Lines, Totals, AccountingEntryId?
```

## Flujo principal

```txt
SalesQuote (opcional) → SalesOrder → SalesDeliveryNote → SalesInvoice
                                                               ↓
                                                          Receivable → CustomerPayment
```

## Eventos publicados

```txt
SalesInvoicePosted(invoiceId, customerId, total, lines)
SalesInvoiceCancelled(invoiceId, reason)
SalesCreditNotePosted(creditNoteId, originalInvoiceId)
```

## Reglas de negocio

- Una factura no puede modificarse una vez en estado `Posted`.
- Una factura cancelada genera crédito automático si hay vencimientos pendientes.
- El número de factura se asigna al pasar a `Posted`, no al crear el borrador.
- El IVA se calcula por línea, no sobre el total (puede haber líneas con IVA distinto).
- Los descuentos se aplican antes del IVA.

## Contabilidad (si módulo activo)

Al publicar `SalesInvoicePosted`, el módulo Accounting:
- Carga la plantilla de asiento para facturas de venta.
- Genera `AccountingEntry` con líneas de cliente (debe) e ingresos/IVA (haber).
- Genera `Receivable` por cada vencimiento según condiciones de pago del cliente.
