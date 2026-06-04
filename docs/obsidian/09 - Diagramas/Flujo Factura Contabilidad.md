---
type: diagram
module: contabilidad
layer: cross
status: implemented
related:
  - Flujo Factura a Contabilidad
  - AccountingEntry
  - AccountingTemplate
---

# Diagrama: Factura a Contabilidad

```mermaid
flowchart TD
    subgraph Ventas
        SI[SalesInvoice\nPosted]
    end

    subgraph Compras
        PI[PurchaseInvoice\nPosted]
    end

    subgraph Motor Contable
        AES[AccountingEntryService]
        T1[Template\nSalesInvoicePosted]
        T2[Template\nPurchaseInvoicePosted]
    end

    subgraph Asiento
        AE[AccountingEntry\nDraft]
        AEL1[Línea: 430 Clientes\nDebe]
        AEL2[Línea: 700 Ventas\nHaber]
        AEL3[Línea: 477 IVA Repercutido\nHaber]
    end

    subgraph Validación
        POST[AccountingEntry\nPosted]
        CHECK{IsBalanced?\nDebe == Haber}
    end

    SI -->|PostSalesInvoice event| AES
    PI -->|PostPurchaseInvoice event| AES
    AES --> T1
    AES --> T2
    T1 --> AE
    AE --> AEL1
    AE --> AEL2
    AE --> AEL3
    AE -->|PostAccountingEntry| CHECK
    CHECK -->|Sí| POST
    CHECK -->|No| ERR[InvalidOperationException]
```

## Invariante de cuadre

| Cuenta | Ventas | Compras |
|--------|--------|---------|
| 430 Clientes | Debe (total) | — |
| 400 Proveedores | — | Haber (total) |
| 700 Ventas | Haber (base) | — |
| 477 IVA Rep. | Haber (IVA) | — |
| 600 Compras | — | Debe (base) |
| 472 IVA Sop. | — | Debe (IVA) |
