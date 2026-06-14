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

> Última actualización: 2026-06-14

## Resueltos en sesión 2026-06-14

| Item | Estado |
|------|--------|
| Tab "Documentos" en CustomerDetail y SupplierDetail (lazy) | ✓ Resuelto — commit `1343246` |
| Módulo AI Governance §42.12 completo (7 capas) | ✓ Resuelto — commit `bf17c08` |
| AIRule, AIKnowledgeBase, AIActionProposal, AIActionApproval, AIExecutionLog | ✓ Resuelto — migración manual `20260614090000_AddAIGovernanceModule` |
| Tests Domain: Document (8), AIActionProposal (13), AIRule (7) | ✓ Resuelto |
| Tests Application: CreateDocumentHandler (4), CreateAIRuleHandler (4) | ✓ Resuelto |
| Vault Obsidian sincronizado (Documentos.md + AIGovernance.md + índices) | ✓ Resuelto |

## Resueltos en sesión 2026-06-13

| Item | Estado |
|------|--------|
| Módulo Documents completo (7 capas) | ✓ Resuelto — commit `a62c926` |
| Document, DocumentType — Domain + 8 handlers + 2 repos + API + UI | ✓ Resuelto — migración manual `20260613120000_AddDocumentsModule` |

---

## Resueltos en sesión 2026-06-09

| Item | Estado |
|------|--------|
| P1 — ModuleRequired en páginas individuales | ✓ Resuelto — `ModuleRouteGuard` en `MainLayout` (cubre todas las rutas automáticamente) |
| P3 — NumberSeries cableado en handlers (13 handlers) | ✓ Resuelto — migración `AddNumberSeriesSeed`, serie consumida en cada Create |
| API REST NumberSeries | ✓ Resuelto — `NumberSeriesController` GET/POST/PUT |
| `RequiresModuleAttribute` en 15 controllers API | ✓ Resuelto — `IFilterFactory` + `ILicenseService` |

---

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

## Prioridad 1 — Tarifas de precio y códigos de artículo por tercero

**Qué falta**: Las entidades `PriceList`, `ItemPrice`, `SupplierItemCode`, `CustomerItemCode` del CLAUDE.md §42.4 no están implementadas.

---

## Prioridad 2 — Importación masiva de datos

**Qué falta**: No hay funcionalidad de importación CSV/Excel para clientes, proveedores, artículos.

---

## Prioridad 3 — Multi-tenant

**Qué falta**: Decisión arquitectónica pendiente (ver CLAUDE.md §47.2). Si se decide multi-tenant, todas las tablas necesitan `TenantId`.

**Impacto estratégico alto** — afecta a toda la base de datos y lógica de acceso.
