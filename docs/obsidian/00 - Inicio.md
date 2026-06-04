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
| Licensing (Planes, Licencias, Módulos de licencia) | **Implementado — contradice CLAUDE.md §46 y §49.3** |
| Despliegue Docker | **Implementado (docker-compose.yml + Dockerfile.api + Dockerfile.web) — contradice CLAUDE.md §30 Fase 7** |

### Tests

- `Debales.Domain.Tests` — Tests de dominio (Customer, Opportunity, Role, User, Entity)
- `Debales.Application.Tests` — Tests de handlers (CreateCustomer, CreateUser) y contratos de repositorio
- `Debales.Integration.Tests` — Smoke tests de solución

---

## Estado declarado en CLAUDE.md

CLAUDE.md §46 declara Licensing y Docker Compose como fases pendientes (Fase 6 y Fase 7). El código demuestra que ambas están implementadas. Ver detalles en [[Contradicciones detectadas]].

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
| [[Contradicciones detectadas]] | CLAUDE.md vs código real |
| [[Inventario técnico]] | Tabla completa de todos los artefactos |
| [[Índice de diagramas]] | Todos los diagramas Mermaid |

---

## Qué revisar primero

1. **[[Contradicciones detectadas]]** — Licensing y Docker están implementados pero CLAUDE.md los marca como pendientes
2. **[[01 - Arquitectura]]** — Para entender la estructura del proyecto
3. **[[Flujo Pedido Venta a Factura]]** — El flujo más representativo del ERP
4. **[[Pendientes priorizados]]** — Huecos reales que quedan por implementar
