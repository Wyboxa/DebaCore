# Módulo Purchasing — Debales

## Estado

Pendiente de implementar (Fase ERP-2 / ERP-3)

## Dependencias

- Core (usuarios, series, auditoría)
- Suppliers (proveedores)
- Catalog (artículos, tarifas, IVA)

## Entidades

```txt
PurchaseOrder
 ├── SupplierId
 ├── NumberSeriesId, Number
 ├── Date, ExpectedDeliveryDate?
 ├── Status (Open | PartiallyReceived | Received | Cancelled)
 ├── Lines: PurchaseOrderLine[]
 └── Totals

PurchaseDeliveryNote
 ├── SupplierId, OrderId?
 ├── NumberSeriesId, Number (interno) + SupplierReference
 ├── Date
 ├── Status (Draft | Posted | Cancelled)
 ├── Lines: PurchaseDeliveryNoteLine[]
 │     ├── OrderLineId?
 │     ├── ItemId, Quantity, WarehouseId
 │     └── StockMovementId? (si Inventory activo)
 └── Totals

PurchaseInvoice
 ├── SupplierId, DeliveryNoteId?
 ├── NumberSeriesId, Number (interno) + SupplierInvoiceNumber
 ├── Date, DueDate
 ├── Status (Draft | Posted | Cancelled)
 ├── Lines: PurchaseInvoiceLine[]
 ├── Totals
 ├── AccountingEntryId? (si Accounting activo)
 └── Payables: Payable[]

PurchaseCreditNote
 ├── OriginalInvoiceId
 ├── NumberSeriesId, Number
 ├── Date, Reason
 ├── Status (Draft | Posted | Cancelled)
 └── Lines, Totals, AccountingEntryId?
```

## Flujo principal

```txt
PurchaseOrder → PurchaseDeliveryNote → PurchaseInvoice
                      ↓                      ↓
               StockMovement (+)         Payable → SupplierPayment
```

## Eventos publicados

```txt
PurchaseInvoicePosted(invoiceId, supplierId, total, lines)
PurchaseInvoiceCancelled(invoiceId, reason)
PurchaseCreditNotePosted(creditNoteId, originalInvoiceId)
PurchaseDeliveryNotePosted(deliveryNoteId, lines)  ← consume Inventory si activo
```

## Reglas de negocio

- El número interno se asigna al registrar la factura, independiente del número del proveedor.
- El número del proveedor (`SupplierInvoiceNumber`) es referencia externa, no clave única.
- Una factura de compra puede llegar sin albarán previo (compra directa).
- El albarán de compra puede generar entrada de stock si el módulo Inventory está activo.

## Contabilidad (si módulo activo)

Al publicar `PurchaseInvoicePosted`, el módulo Accounting:
- Genera `AccountingEntry` con líneas de proveedor (haber) y gasto/IVA soportado (debe).
- Genera `Payable` por cada vencimiento según condiciones de pago del proveedor.
