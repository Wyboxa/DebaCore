---
type: flow
module: ventas
layer: cross
status: implemented
source:
  - src/Debales.Domain/Sales/Receivable.cs
  - src/Debales.Domain/Sales/CustomerPayment.cs
  - src/Debales.Domain/Purchasing/Payable.cs
  - src/Debales.Domain/Purchasing/SupplierPayment.cs
related:
  - Ventas
  - Compras
  - SalesInvoice
  - PurchaseInvoice
  - Receivable
  - CustomerPayment
  - Payable
  - SupplierPayment
---

# Flujo: Cobro y Pago

## Diagrama

```mermaid
graph TD
    A[SalesInvoice\nPosted] -->|Genera| B[Receivable\nPending]
    B -->|Cobro parcial| C[Receivable\nPartial]
    C -->|Cobro total| D[Receivable\nSettled]
    B -->|Cobro total| D
    B -->|Impago| E[Receivable\nDefaulted]
    
    F[CustomerPayment] -->|Liquida| B
    F -->|Liquida| C
    
    G[PurchaseInvoice\nPosted] -->|Genera| H[Payable\nPending]
    H -->|Pago parcial| I[Payable\nPartial]
    I -->|Pago total| J[Payable\nSettled]
    H -->|Pago total| J
    
    K[SupplierPayment] -->|Liquida| H
    K -->|Liquida| I
```

## Estados de Receivable (Vencimiento de cobro)

| Estado | Descripción |
|--------|-------------|
| `Pending` | Pendiente de cobro |
| `Partial` | Cobrado parcialmente |
| `Settled` | Cobrado completamente |
| `Defaulted` | Impagado declarado |
| `Cancelled` | Cancelado |

## Estados de Payable (Vencimiento de pago)

Mismo modelo que Receivable: `Pending | Partial | Settled | Defaulted | Cancelled`

## Flujo de cobro

### 1. Vencimiento generado

Al contabilizar una `SalesInvoice` (`PostSalesInvoiceHandler`), se genera automáticamente un `Receivable` con:
- Importe: Total de la factura
- Fecha vencimiento: `DueDate` de la factura
- Estado: `Pending`

### 2. Registrar cobro

- Handler: `CreateCustomerPaymentHandler`
- Entidad: `CustomerPayment`
- Liquida uno o varios `Receivable`
- Puede ser pago parcial o total

## Flujo de pago

### 1. Vencimiento generado

Al contabilizar una `PurchaseInvoice`, se genera un `Payable`.

### 2. Registrar pago

- Handler: `CreateSupplierPaymentHandler`
- Entidad: `SupplierPayment`
- Liquida uno o varios `Payable`

## Integración contable (pendiente)

La generación de asientos desde cobros/pagos no está confirmada en el código actual. El `AccountingEntryService` tiene plantillas para facturas pero no se confirmó la existencia de plantillas para cobros/pagos.
