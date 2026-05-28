# Roadmap — Debales

> Fuente de verdad: `roadmap.md` en la raíz del proyecto.
> Este fichero es un resumen de referencia. Para el roadmap completo y actualizado, ver `/roadmap.md`.
>
> Última actualización: 2026-05-28
> Estado de partida: **Fases 0–3 completadas** (solución .NET 8, Core, CRM, API, UI Blazor, 31 tests)

---

## Prioridad 0 — Requisitos críticos (bloquean uso real)

| Tarea | Descripción |
|-------|-------------|
| P0-1 | Autenticación y autorización (JWT o cookie, login en Blazor) |
| P0-2 | Seed de datos iniciales (usuario admin, roles base, módulos registrados) |
| P0-3 | Middleware de errores global (Problem Details RFC 7807) |
| P0-4 | Corregir bug de autorización en oportunidades |
| P0-5 | Añadir `GET /api/customers/{id}/notes` |

---

## Fases técnicas base

| Fase | Estado | Objetivo |
|------|--------|---------|
| Fase 0 — Cimientos | ✅ Completada | Repo, CLAUDE.md, documentación base, arquitectura |
| Fase 1 — Solución base .NET | ✅ Completada | Solución .NET 8, 10 proyectos, capas limpias |
| Fase 2 — Módulo Core | ✅ Completada | Usuarios, roles, permisos, auditoría básica |
| Fase 3 — Módulo CRM | ✅ Completada | Clientes, contactos, actividades, notas, oportunidades, API REST, UI Blazor |
| Fase 4 — IA documental | ⏳ Siguiente | Chat IA con contexto CRM, resumen de cliente, propuestas |
| Fase 5 — IA técnica supervisada | 🔜 Futura | Generación de código, ramas, PRs supervisados |
| Fase 6 — Licenciamiento | 🔜 Futura | Planes, módulos contratados, activación |
| Fase 7 — Despliegue local | 🔜 Futura | Docker Compose, configuración por cliente, backups |

---

## Fases ERP

| Fase | Estado | Objetivo |
|------|--------|---------|
| ERP-1 — Proveedores y catálogo | 🔜 Futura | Supplier, Catalog, Item, TaxType, PriceList, NumberSeries |
| ERP-2 — Ventas y compras básicas | 🔜 Futura | SalesOrder, PurchaseOrder, albaranes (sin contabilizar) |
| ERP-3 — Facturación | 🔜 Futura | SalesInvoice, PurchaseInvoice, Receivable, Payable, cobros/pagos |
| ERP-4 — Almacén | 🔜 Futura | Warehouse, StockMovement, StockBalance |
| ERP-5 — Contabilidad mínima | 🔜 Futura | ChartOfAccounts, FiscalYear, AccountingEntry, asientos automáticos |
| ERP-6 — IA sobre ERP | 🔜 Futura | IA con contexto ventas/compras/contabilidad, propuestas de asientos |

---

## Decisiones pendientes que desbloquean fases

| Decisión | Bloquea |
|----------|---------|
| Esquema de autenticación (JWT vs cookie) | P0-1 |
| Proveedor IA para desarrollo | Fase 4 |
| Multi-tenant desde inicio o diferir | Todas las tablas futuras |
| Plan contable por defecto (PGC España) | ERP-5 |
| Motor contable propio vs librería | ERP-5 |
| Formas de pago y remesas (SEPA) | ERP-3 |

Ver detalle en `roadmap.md` (raíz) y `docs/decisions/`.
