# ADR-0004 — Separación entre documentos operativos y documentos contables

## Estado

Aceptada — 2026-05-28

## Contexto

En un ERP, es habitual mezclar la lógica operativa (pedidos, albaranes, facturas) con la lógica
contable (asientos, vencimientos, cobros). Esta mezcla produce:

- Dificultad para cerrar ejercicios sin bloquear operaciones.
- Imposibilidad de tener contabilidad opcional o diferida.
- Acoplamiento entre módulos que deberían ser independientes.
- Complejidad en auditorías y rectificaciones.

## Decisión

Los documentos operativos y los documentos contables son entidades distintas y pertenecen a capas distintas.

Un documento operativo (como `SalesInvoice`) **puede generar** un documento contable (como `AccountingEntry`),
pero no **es** un documento contable.

La relación es unidireccional: operativo → genera → contable.
El dominio contable no depende del dominio operativo para validar su integridad.

### Documentos operativos

```txt
SalesQuote, SalesOrder, SalesDeliveryNote, SalesInvoice, SalesCreditNote
PurchaseOrder, PurchaseDeliveryNote, PurchaseInvoice, PurchaseCreditNote
StockMovement, StockAdjustment
```

### Documentos contables

```txt
AccountingEntry, AccountingEntryLine
Receivable, CustomerPayment
Payable, SupplierPayment
Remittance
```

## Consecuencias

- Los módulos de Ventas y Compras pueden funcionar sin módulo de Contabilidad activo.
- La contabilización es un paso explícito, no automático en la creación del documento.
- Los cierres de periodo/ejercicio afectan solo al módulo Accounting.
- Las rectificativas operativas y las contables son entidades separadas pero relacionadas.
- El módulo de Contabilidad puede activarse en cualquier fase sin reescribir Ventas/Compras.

## Alternativas consideradas

**Alternativa: Documento operativo con campos contables integrados**
- Rechazada. Impide desactivar contabilidad sin afectar operaciones.

**Alternativa: Vista contable generada desde documento operativo**
- Rechazada. No permite asientos manuales ni ajustes contables independientes.
