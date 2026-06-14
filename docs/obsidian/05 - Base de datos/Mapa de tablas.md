---
type: database
module: cross
layer: database
status: implemented
source:
  - src/Debales.Infrastructure/Persistence/ApplicationDbContext.cs
  - src/Debales.Infrastructure/Persistence/Migrations/
related:
  - DbContext
  - Relaciones de base de datos
---

# Mapa de tablas SQL

Base de datos: `DebalesDb` (SQL Server LocalDB en desarrollo, SQL Server 2022 en Docker)

## Tablas por módulo

### Core

| Tabla SQL | Entidad | Descripción |
|-----------|---------|-------------|
| `Users` | [[User]] | Usuarios del sistema |
| `Roles` | [[Role]] | Roles del sistema |
| `Permissions` | [[Permission]] | Permisos atómicos |
| `UserRoles` | [[UserRole]] | Asignación usuario-rol |
| `RolePermissions` | [[RolePermission]] | Asignación rol-permiso |
| `SystemModules` | [[SystemModule]] | Módulos registrados |
| `AuditEntries` | AuditEntry | Log de auditoría |

### CRM

| Tabla SQL | Entidad |
|-----------|---------|
| `Customers` | [[Customer]] |
| `Contacts` | [[Contact]] |
| `Activities` | [[Activity]] |
| `Notes` | [[Note]] |
| `Opportunities` | [[Opportunity]] |

### Suppliers

| Tabla SQL | Entidad |
|-----------|---------|
| `Suppliers` | [[Supplier]] |

### Catalog

| Tabla SQL | Entidad |
|-----------|---------|
| `Items` | [[Item]] |
| `ItemFamilies` | [[ItemFamily]] |
| `UnitsOfMeasure` | [[UnitOfMeasure]] |
| `TaxTypes` | [[TaxType]] |

### Sales

| Tabla SQL | Entidad |
|-----------|---------|
| `SalesOrders` | [[SalesOrder]] |
| `SalesOrderLines` | SalesOrderLine |
| `SalesDeliveryNotes` | SalesDeliveryNote |
| `SalesDeliveryNoteLines` | SalesDeliveryNoteLine |
| `SalesInvoices` | [[SalesInvoice]] |
| `SalesInvoiceLines` | SalesInvoiceLine |
| `SalesCreditNotes` | SalesCreditNote |
| `SalesCreditNoteLines` | SalesCreditNoteLine |
| `Receivables` | Receivable |
| `CustomerPayments` | CustomerPayment |

### Purchasing

| Tabla SQL | Entidad |
|-----------|---------|
| `PurchaseOrders` | [[PurchaseOrder]] |
| `PurchaseOrderLines` | PurchaseOrderLine |
| `PurchaseDeliveryNotes` | PurchaseDeliveryNote |
| `PurchaseDeliveryNoteLines` | PurchaseDeliveryNoteLine |
| `PurchaseInvoices` | [[PurchaseInvoice]] |
| `PurchaseInvoiceLines` | PurchaseInvoiceLine |
| `PurchaseCreditNotes` | PurchaseCreditNote |
| `PurchaseCreditNoteLines` | PurchaseCreditNoteLine |
| `Payables` | Payable |
| `SupplierPayments` | SupplierPayment |

### Inventory

| Tabla SQL | Entidad |
|-----------|---------|
| `Warehouses` | [[Warehouse]] |
| `WarehouseLocations` | WarehouseLocation |
| `StockMovements` | StockMovement |
| `StockBalances` | StockBalance |

### Accounting

| Tabla SQL | Entidad |
|-----------|---------|
| `Accounts` | [[Account]] |
| `FiscalYears` | [[FiscalYear]] |
| `FiscalPeriods` | FiscalPeriod |
| `AccountingJournals` | AccountingJournal |
| `AccountingEntries` | [[AccountingEntry]] |
| `AccountingEntryLines` | AccountingEntryLine |
| `AccountingTemplates` | AccountingTemplate |
| `AccountingTemplateLines` | AccountingTemplateLine |

### Licensing

| Tabla SQL | Entidad |
|-----------|---------|
| `SubscriptionPlans` | SubscriptionPlan |
| `Licenses` | [[License]] |
| `LicenseModules` | [[LicenseModule]] |

### Documents

| Tabla SQL | Entidad |
|-----------|---------|
| `Documents` | Document |
| `DocumentTypes` | DocumentType |

### AI Governance

| Tabla SQL | Entidad | Tipo base |
|-----------|---------|-----------|
| `AIRules` | AIRule | AuditableEntity (soft-delete) |
| `AIKnowledgeBases` | AIKnowledgeBase | AuditableEntity (soft-delete) |
| `AIActionProposals` | AIActionProposal | AuditableEntity (soft-delete) |
| `AIActionApprovals` | AIActionApproval | Entity (inmutable, sin soft-delete) |
| `AIExecutionLogs` | AIExecutionLog | Entity (inmutable, sin soft-delete) |

**Total tablas: ~63**

## Relaciones clave de trazabilidad (navegación)

| Desde | Campo FK | Hacia | Uso |
|-------|----------|-------|-----|
| `SalesDeliveryNote` | `SalesOrderId` | `SalesOrder` | Trazabilidad albarán → pedido |
| `SalesInvoice` | `SalesDeliveryNoteId` | `SalesDeliveryNote` | Trazabilidad factura → albarán |
| `PurchaseDeliveryNote` | `PurchaseOrderId` | `PurchaseOrder` | Trazabilidad albarán → pedido |
| `PurchaseInvoice` | `PurchaseDeliveryNoteId` | `PurchaseDeliveryNote` | Trazabilidad factura → albarán |
| `StockMovement` | `Reference` | string | Referencia al albarán origen (`"ALV-001"`) |
| `AccountingEntry` | — | `SalesInvoice` / `PurchaseInvoice` | Asiento generado por factura |

## Nota: StockMovements generados automáticamente

A partir de 2026-06-05, `PostSalesDeliveryNoteHandler` y `PostPurchaseDeliveryNoteHandler` insertan filas en `StockMovements` y actualizan `StockBalances` de forma automática. El campo `Reference` del movimiento contiene el número del albarán origen.

## Campos comunes (AuditableEntity)

Todas las entidades que heredan de `AuditableEntity` tienen:

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Id` | `uniqueidentifier` | PK GUID |
| `CreatedAt` | `datetime2` | Fecha creación |
| `CreatedBy` | `nvarchar` | Usuario que creó |
| `UpdatedAt` | `datetime2?` | Fecha última modificación |
| `UpdatedBy` | `nvarchar?` | Usuario que modificó |
| `IsDeleted` | `bit` | Soft-delete |
| `DeletedAt` | `datetime2?` | Fecha de borrado lógico |
| `DeletedBy` | `nvarchar?` | Usuario que borró |

`AIActionApproval` y `AIExecutionLog` heredan de `Entity` (sin campos de soft-delete) — son registros inmutables de auditoría.
