---
type: audit
module: cross
layer: cross
status: not_confirmed
related:
  - Contradicciones detectadas
  - Huecos funcionales
  - Deuda técnica
  - Mejoras UI
---

# Pendientes priorizados

## Prioridad 1 — Corrección de CLAUDE.md

Actualizar CLAUDE.md para reflejar que Licensing (Fase 6) y Docker Compose (Fase 7) están implementados. Ver [[Contradicciones detectadas]].

**Acción**: Actualizar secciones §6, §46, §49.3 de CLAUDE.md.

---

## Prioridad 2 — Integración asientos contables desde cobros/pagos

**Qué falta**: Cuando se registra un `CustomerPayment` o `SupplierPayment`, no se confirma que se genere asiento contable automático.

Las plantillas de asiento (`AccountingTemplate`) están definidas para `SalesInvoicePosted` y `PurchaseInvoicePosted`, pero no se encuentran plantillas para cobros/pagos en `AccountingSeeds`.

**Impacto**: La contabilidad queda incompleta — los cobros y pagos no tienen reflejo contable automático.

---

## Prioridad 3 — Validación de licencia en middleware

**Qué falta**: La licencia se puede ver y activar desde la UI, pero no hay middleware que bloquee el acceso si la licencia está caducada o suspendida.

**Impacto**: La plataforma funciona sin licencia activa. El módulo de licenciamiento no tiene efecto práctico en el acceso.

---

## Prioridad 4 — Integración almacén con albaranes

**Qué falta**: Los albaranes de venta (salida) y compra (entrada) no generan movimientos de stock automáticos en `StockMovements`.

**Impacto**: El inventario solo se actualiza con movimientos manuales. El stock no refleja la operativa real de ventas y compras.

---

## Prioridad 5 — Gestión de usuarios desde UI

**Qué falta**: No hay páginas Blazor para crear, listar o gestionar usuarios y roles. Solo existe el endpoint API `POST /api/users`.

**Impacto**: Solo se puede crear usuarios vía API directamente.

---

## Prioridad 6 — Presupuestos de venta (SalesQuote)

**Qué falta**: La entidad `SalesQuote` está en el catálogo conceptual del CLAUDE.md pero no existe en el dominio ni en la base de datos.

---

## Prioridad 7 — Informes contables

**Qué falta**: No hay endpoints ni páginas para:
- Balance de situación
- Cuenta de pérdidas y ganancias
- Balance de comprobación de sumas y saldos
- Libro diario

---

## Prioridad 8 — Tarifas de precio y códigos de artículo por tercero

**Qué falta**: Las entidades `PriceList`, `ItemPrice`, `SupplierItemCode`, `CustomerItemCode` del CLAUDE.md §42.4 no están implementadas en el dominio.

---

## Prioridad 9 — Importación masiva de datos

**Qué falta**: No hay funcionalidad de importación CSV/Excel para clientes, proveedores, artículos.

---

## Prioridad 10 — Dashboard analítico

**Qué falta**: La página `/analitica` existe pero el nivel de implementación no se confirmó. No hay KPIs ni gráficos confirmados.
