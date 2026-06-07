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

> Última actualización: 2026-06-07

## Resueltos en sesión 2026-06-07

| Item | Estado |
|------|--------|
| AccountCode cascade Customer/Supplier | ✓ Resuelto — commit `fcd2c65` |
| Sistema auditoría automática (AuditEntry + UI) | ✓ Resuelto — commit `130c24d` |
| Informes contables con filtro ejercicio/período | ✓ Resuelto — commit `f239efe` |
| Tutorial guiado (TutorialService + TutorialOverlay) | ✓ Resuelto — commit `d9ee4ae` |
| Asientos desde cobros/pagos | ✓ Resuelto — commit `0f5cda3` |
| Dashboard KPIs + alertas pagos vencidos | ✓ Resuelto — commit `dba130b` |
| P7 — Tests AssignRoleHandler (4) + DeactivateUserHandler (3) | ✓ Resuelto — commit `91048a8` |
| NumberSeries (series documentales, UI `/configuracion/series`) | ✓ Resuelto — commit `91048a8`, migración 13 |
| Fix `IHttpContextAccessor` en `Debales.Api/Program.cs` | ✓ Resuelto — commit `91048a8` |
| Duplicate `@using Debales.Application.Licensing` en `_Imports.razor` | ✓ Resuelto — commit `91048a8` |

---

## Resueltos en sesión 2026-06-05

| Item | Estado |
|------|--------|
| P1 — Actualizar CLAUDE.md (Licensing, Docker como completos) | ✓ Resuelto — commit `f434a9f` |
| P3 — Validación de licencia en middleware | ✓ Resuelto — `ModuleRequired.razor` en hub pages |
| P4 — Integración almacén con albaranes | ✓ Resuelto — `PostSalesDeliveryNoteHandler` crea movimientos Out; `PostPurchaseDeliveryNoteHandler` crea In |
| P5 — Gestión de usuarios desde UI | ✓ Resuelto — `/configuracion/usuarios` + `/configuracion/usuarios/{id}` |
| P10 — Dashboard analítico | ✓ Resuelto — `Home.razor` con 6 KPIs reales, alertas, pedidos e facturas recientes |
| Flujo espejo Compras (Generar albarán → Generar factura) | ✓ Resuelto — `AlbaranCompraDetalle.razor` con selector de almacén + botón generar factura |
| PDF export facturas | ✓ Resuelto — QuestPDF en `InvoicePdfGenerator`, endpoints `/descargar/factura-{venta,compra}/{id}` |
| Configuración con datos reales | ✓ Resuelto — `Configuracion.razor` muestra versión, usuarios activos, roles, estado de licencia |

---

## Prioridad 1 — ModuleRequired en páginas de lista (no solo hubs)

**Qué falta**: `ModuleRequired` está aplicado en los hubs (`/ventas`, `/compras`, `/inventario`, `/facturacion`, `/analitica`), pero no en las páginas de lista y detalle individuales (`/ventas/pedidos`, `/contabilidad/asientos`, etc.).

**Impacto**: Un usuario sin módulo Sales puede navegar directamente a `/ventas/pedidos` sin bloqueo.

**Solución**: Envolver `@body` en cada página de lista/detalle con `<ModuleRequired Module="Sales">`.

---

## Prioridad 2 — Tarifas de precio y códigos de artículo por tercero

**Qué falta**: Las entidades `PriceList`, `ItemPrice`, `SupplierItemCode`, `CustomerItemCode` del CLAUDE.md §42.4 no están implementadas.

---

## Prioridad 3 — NumberSeries en handlers de documentos

**Qué falta**: `NumberSeries.Consume()` existe y funciona, pero no está cableado en los handlers de creación de documentos (`CreateSalesInvoiceHandler`, `CreateSalesOrderHandler`, `CreatePurchaseInvoiceHandler`, etc.).

**Impacto**: Los números de factura/pedido/albarán siguen siendo entrada manual o secuencial simple. Las series configuradas en `/configuracion/series` no se usan todavía.

**Estimación**: Media. Cada handler necesita recibir `INumberSeriesRepository` y llamar a `Consume()` con el código de serie correcto.

---

## Prioridad 4 — Importación masiva de datos

**Qué falta**: No hay funcionalidad de importación CSV/Excel para clientes, proveedores, artículos.

---

## Prioridad 5 — Multi-tenant

**Qué falta**: Decisión arquitectónica pendiente (ver CLAUDE.md §47.2). Si se decide multi-tenant, todas las tablas necesitan `TenantId`.

**Impacto estratégico alto** — afecta a toda la base de datos y lógica de acceso.
