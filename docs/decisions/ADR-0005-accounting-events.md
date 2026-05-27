# ADR-0005 — Contabilidad basada en eventos de negocio

## Estado

Aceptada — 2026-05-28

## Contexto

Existen dos aproximaciones habituales para generar asientos contables en un ERP:

1. **Contabilización inline**: el handler que crea la factura también crea el asiento.
2. **Contabilización por eventos**: el documento operativo publica un evento, el módulo contable lo procesa.

La primera opción es más simple al inicio pero acopla los módulos.
La segunda requiere más infraestructura pero mantiene los módulos aislados.

## Decisión

La contabilidad se genera desde eventos de negocio confirmados.

Cada evento representa una acción operativa que tiene relevancia contable:

```txt
SalesInvoicePosted         → genera asiento de factura de venta
SalesInvoiceCancelled      → genera asiento de anulación
PurchaseInvoicePosted      → genera asiento de factura de compra
PurchaseInvoiceCancelled   → genera asiento de anulación
CustomerPaymentConfirmed   → genera asiento de cobro
SupplierPaymentConfirmed   → genera asiento de pago
SalesCreditNotePosted      → genera asiento de rectificativa
PurchaseCreditNotePosted   → genera asiento de rectificativa
StockAdjustmentConfirmed   → puede generar asiento de regularización
FiscalYearClosed           → bloquea periodos
FiscalYearOpened           → habilita periodos
```

Los eventos son publicados por el módulo operativo.
El módulo de Contabilidad se suscribe a los eventos que le corresponden.
Ningún módulo operativo llama directamente a `AccountingEntry`.

### Implementación inicial

En la fase inicial (Fase ERP-5), los eventos pueden resolverse via llamada directa al handler contable
desde el handler operativo, sin mensajería. La abstracción queda preparada para evolucionar.

El contrato es el evento: no importa si se procesa sincrónicamente o por cola.

## Consecuencias

- El módulo de Contabilidad puede activarse o desactivarse sin cambiar código de Ventas.
- Los asientos automáticos y manuales coexisten en el mismo sistema.
- Los eventos quedan registrados y son auditables.
- Si el módulo de Contabilidad no está activo, los eventos se descartan o acumulan.
- Facilita tests unitarios: se prueba que el evento se publica, no que el asiento se crea.

## Alternativas consideradas

**Alternativa: Generación inline en el handler de factura**
- Rechazada por acoplamiento. Impide desactivar contabilidad.

**Alternativa: Cron batch de contabilización**
- Rechazada para la base. Añade latencia y complejidad de coordinación.
  Puede ser complemento para recontabilizaciones, no el mecanismo principal.
