---
type: diagram
module: compras
layer: cross
status: implemented
related:
  - Flujo Pedido Compra a Factura
  - PurchaseOrder
  - PurchaseInvoice
---

# Diagrama: Flujo de Compras

```mermaid
flowchart TD
    A([Necesidad de compra]) --> B[PurchaseOrder\nDraft]
    B -->|Confirm| C[PurchaseOrder\nConfirmed]
    C -->|Recepción| D[PurchaseDeliveryNote\nDraft]
    D -->|Post| E[PurchaseDeliveryNote\nPosted]
    E -->|Factura proveedor| F[PurchaseInvoice\nDraft]
    F -->|Post| G[PurchaseInvoice\nPosted]
    G --> H[Payable\nPending]
    H -->|SupplierPayment| I{¿Pagado todo?}
    I -->|Sí| J[Payable\nSettled]
    I -->|Parcial| K[Payable\nPartial]
    K -->|Otro pago| I

    B -->|Cancel| L([PurchaseOrder Cancelled])
    F -->|Cancel Draft| M([PurchaseInvoice Cancelled])
    G -->|PurchaseCreditNote| N([Rectificativa Posted])

    style G fill:#6B9CA9,color:#fff
    style J fill:#28a745,color:#fff
```

## Diferencia clave con Ventas

`PurchaseInvoice` tiene el campo adicional `SupplierInvoiceNumber` para registrar el número de factura del proveedor. No existe generación automática en lote como en ventas (`BatchGenerateDocuments`).
