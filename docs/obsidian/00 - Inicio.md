---
type: index
module: cross
layer: cross
status: implemented
source:
  - CLAUDE.md
  - src/Debales.Infrastructure/Persistence/Migrations
  - tests/
related:
  - 01 - Arquitectura
  - Índice de módulos
  - Inventario técnico
---

# Debales — Inicio

## Resumen del proyecto

Debales es una plataforma CRM/ERP modular con inteligencia artificial supervisada, construida en .NET 8. El objetivo es proporcionar a las empresas un sistema adaptable a sus procesos internos, donde una IA conocedora de los módulos contratados asiste al usuario bajo supervisión humana obligatoria.

**Stack:** .NET 8 · C# · ASP.NET Core · Blazor Server · Entity Framework Core · SQL Server · JWT

---

## Estado real según código

### Migraciones aplicadas (confirmadas en código)

| Orden | Nombre | Fecha |
|-------|--------|-------|
| 1 | InitialCreate | 2026-05-27 |
| 2 | AddCrmModule | 2026-05-27 |
| 3 | AddCustomerEmail | 2026-05-28 |
| 4 | AddSuppliersModule | 2026-05-28 |
| 5 | AddCatalogModule | 2026-05-28 |
| 6 | AddERP2Module | 2026-05-29 |
| 7 | AddERP3Module | 2026-06-01 |
| 8 | AddERP4Module | 2026-06-01 |
| 9 | AddAccountingModule | 2026-06-02 |
| 10 | AddLicensingModule | 2026-06-04 |
| 11 | AddSalesQuoteModule | 2026-06-04 |
| 12 | AddPaymentAccountingTemplates | 2026-06-07 |

### Módulos con código real

| Módulo | Estado real |
|--------|-------------|
| Core (Usuarios, Roles, Permisos) | Implementado |
| CRM (Clientes, Contactos, Actividades, Notas, Oportunidades) | Implementado |
| Suppliers (Proveedores) | Implementado |
| Catalog (Artículos, Familias, UoM, TaxType) | Implementado |
| Sales (Pedidos, Albaranes, Facturas, Rectificativas, Vencimientos, Cobros) | Implementado |
| Purchasing (Pedidos, Albaranes, Facturas, Rectificativas, Vencimientos, Pagos) | Implementado |
| Inventory (Almacenes, Ubicaciones, Movimientos, Saldos) | Implementado |
| Accounting (Plan Contable, Ejercicios, Diarios, Asientos) | Implementado |
| AI ERP-6 (Chat ERP, Anomalías, Resúmenes cliente/proveedor) | Implementado |
| Licensing (Planes, Licencias, Módulos de licencia) | Implementado |
| Despliegue Docker | Implementado (docker-compose.yml + Dockerfile.api + Dockerfile.web) |
| SalesQuote (Presupuestos de venta) | Implementado — migración `AddSalesQuoteModule` — 2026-06-04 |
| Informes contables (Balance comprobación, Libro diario, Balance situación) | Implementado — 2026-06-05 |
| User management UI (`/configuracion/usuarios`) | Implementado — 2026-06-05 |
| Stock auto-movimientos desde albaranes | Implementado — 2026-06-05 |
| Flujo espejo Compras (albarán → factura) | Implementado — 2026-06-05 |
| PDF export facturas (QuestPDF) | Implementado — 2026-06-05 |
| ModuleRequired (enforcement de licencia en UI) | Implementado — 2026-06-05 |
| Asientos automáticos desde cobros/pagos | Implementado — 2026-06-06 |
| GitHub remoto configurado (Wyboxa/DebaCore) | Configurado — 2026-06-06 |
| Tutorial guiado activable (TutorialService + TutorialOverlay) | Implementado — 2026-06-07 |

### Tests

**Total: 58 tests — 0 errores**

| Suite | Tests | Última actualización |
|-------|-------|---------------------|
| `Debales.Domain.Tests` | 31 — Customer, Opportunity, Role, User, UserExtended, SalesDeliveryNote | 2026-06-05 |
| `Debales.Application.Tests` | 26 — CreateCustomer, CreateUser, ChangePassword, PostSalesDeliveryNote, GenerateInvoiceFromPurchaseDeliveryNote, PostPurchaseDeliveryNote, **CreateCustomerPayment, CreateSupplierPayment** | 2026-06-06 |
| `Debales.Integration.Tests` | 1 — Smoke tests de solución | 2026-06-04 |

---

## Estado declarado en CLAUDE.md

CLAUDE.md actualizado el 2026-06-04 — refleja correctamente el estado implementado de Licensing (Fase 6), Docker (Fase 7) y los módulos ERP-1 a ERP-6. Las contradicciones anteriores han sido resueltas. Ver [[Contradicciones detectadas]].

---

## Mapa de navegación del vault

| Sección | Descripción |
|---------|-------------|
| [[01 - Arquitectura]] | Capas, dependencias, flujo típico |
| [[Índice de módulos]] | Todos los módulos con estado |
| [[Índice de entidades]] | Todas las entidades de dominio |
| [[Índice de flujos]] | Flujos end-to-end documentados |
| [[Índice base de datos]] | DbContext, migraciones, seeds |
| [[Índice API]] | Controllers y endpoints |
| [[Índice UI Blazor]] | Páginas Razor y rutas |
| [[Pendientes priorizados]] | Lo que falta y su prioridad |
| [[Contradicciones detectadas]] | CLAUDE.md vs código real — protocolo activo |
| [[Huecos funcionales]] | Entidades e integraciones pendientes con estado |
| [[09 - Ideas y decisiones]] | Registro de todas las ideas surgidas en desarrollo |
| [[Inventario técnico]] | Tabla completa de todos los artefactos |
| [[Índice de diagramas]] | Todos los diagramas Mermaid |

---

## Qué revisar primero

1. **[[Contradicciones detectadas]]** — Licensing y Docker están implementados pero CLAUDE.md los marca como pendientes
2. **[[01 - Arquitectura]]** — Para entender la estructura del proyecto
3. **[[Flujo Pedido Venta a Factura]]** — El flujo más representativo del ERP
4. **[[Pendientes priorizados]]** — Huecos reales que quedan por implementar
