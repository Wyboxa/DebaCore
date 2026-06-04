---
type: audit
module: cross
layer: cross
status: implemented
related:
  - Inventario técnico
  - 00 - Inicio
---

# Cobertura de documentación del vault

## Notas generadas por sección

| Sección | Notas | Estado |
|---------|-------|--------|
| Raíz | 3 (Inicio, Arquitectura, Cómo usar) | Completo |
| 02 - Módulos | 12 (Índice + 11 módulos) | Completo |
| 03 - Entidades | 10 (Índice + 9 entidades representativas) | Parcial — ver nota |
| 04 - Flujos | 8 (Índice + 7 flujos) | Completo |
| 05 - Base de datos | 6 (Índice + 5 notas) | Completo |
| 06 - API | 5 (Índice + Mapa + 3 controllers detallados) | Parcial — ver nota |
| 07 - UI Blazor | 2 (Índice + Rutas) | Parcial — ver nota |
| 08 - Pendientes | 7 notas | Completo |
| 09 - Diagramas | 10 (Índice + 9 diagramas) | Completo |
| 10 - Auditoría | 2 (Inventario + Cobertura) | Completo |

**Total notas generadas: ~65**

## Notas de entidades no creadas individualmente

Se crearon notas individuales para las entidades más importantes. Las entidades restantes están documentadas en la nota de su módulo correspondiente:

| Entidades documentadas en nota de módulo | Módulo |
|------------------------------------------|--------|
| Contact, Activity, Note, Opportunity | CRM.md |
| SupplierAddress | Suppliers.md |
| ItemFamily, UnitOfMeasure, TaxType | Catalogo.md |
| SalesOrderLine, SalesDeliveryNote, SalesCreditNote, Receivable, CustomerPayment | Ventas.md |
| PurchaseOrder, PurchaseDeliveryNote, PurchaseInvoice, PurchaseCreditNote, Payable, SupplierPayment | Compras.md |
| WarehouseLocation, StockMovement, StockBalance | Inventario.md |
| FiscalYear, FiscalPeriod, AccountingJournal, AccountingEntryLine, AccountingTemplate | Contabilidad.md |
| LicenseModule, SubscriptionPlan | Licenciamiento.md |
| User, Role, Permission, AuditEntry, SystemModule | Core.md |

## Controllers sin nota individual

Los controllers cubiertos solo en el Mapa API:
- AuthController, UsersController, HealthController
- CustomersController, SuppliersController, ItemsController
- SalesOrdersController y familia (en Mapa API)
- PurchaseOrdersController y familia (en Mapa API)
- WarehousesController, StockMovementsController
- SubscriptionPlansController

## Páginas Blazor sin nota individual

Las páginas están documentadas en el Índice UI Blazor y Rutas Blazor. Las páginas con más lógica documentadas con referencia:
- `Pedidos.razor` — lógica de creación con líneas documentada en [[Ventas]]
- `IA.razor` — lógica de 4 tabs documentada en [[IA]]
- `Licencia.razor` — lógica de activación documentada en [[Licenciamiento]]
- `PlanContable.razor` — documentada en [[Contabilidad]]

## Resumen de cobertura

| Elemento | Total en código | Documentados | Cobertura |
|----------|-----------------|--------------|-----------|
| Módulos | 12 | 12 | 100% |
| Entidades (notas individuales) | 55 | 9 | 16% (resto en módulo) |
| Entidades (referenciadas) | 55 | 55 | 100% |
| Controllers | 22 | 22 | 100% (Mapa API) |
| Páginas Blazor | 44 | 44 | 100% (Índice + Rutas) |
| Flujos | 7 | 7 | 100% |
| Diagramas | 9 | 9 | 100% |
| Migraciones | 10 | 10 | 100% |
