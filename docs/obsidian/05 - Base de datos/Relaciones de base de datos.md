---
type: database
module: cross
layer: database
status: implemented
source:
  - src/Debales.Infrastructure/Persistence/ApplicationDbContext.cs
  - src/Debales.Infrastructure/Persistence/Configurations/
related:
  - DbContext
  - Mapa de tablas
---

# Relaciones de base de datos

Relaciones con FK confirmadas en código (propiedades de navegación o configuraciones EF).

## Relaciones 1:N

| Padre | Hijo | FK en hijo | Confirmación |
|-------|------|-----------|--------------|
| Customer | Contact | `CustomerId` | Explícita |
| Customer | Activity | `CustomerId` | Explícita |
| Customer | Note | `CustomerId` | Explícita |
| Customer | Opportunity | `CustomerId` | Explícita |
| Customer | SalesOrder | `CustomerId` | Explícita |
| Customer | SalesInvoice | `CustomerId` | Explícita |
| Supplier | PurchaseOrder | `SupplierId` | Explícita |
| Supplier | PurchaseInvoice | `SupplierId` | Explícita |
| Item | SalesOrderLine | `ItemId` | Explícita |
| Item | PurchaseOrderLine | `ItemId` | Explícita |
| Item | StockMovement | `ItemId` | Explícita |
| Item | StockBalance | `ItemId` | Explícita |
| ItemFamily | Item | `FamilyId` | Explícita |
| UnitOfMeasure | Item | `UnitOfMeasureId` | Explícita |
| TaxType | Item | `TaxTypeId` | Explícita |
| SalesOrder | SalesOrderLine | `SalesOrderId` | Explícita |
| SalesOrder | SalesDeliveryNote | `SalesOrderId` | Explícita |
| SalesDeliveryNote | SalesDeliveryNoteLine | `SalesDeliveryNoteId` | Explícita |
| SalesDeliveryNote | SalesInvoice | `SalesDeliveryNoteId` | Explícita |
| SalesInvoice | SalesInvoiceLine | `SalesInvoiceId` | Explícita |
| PurchaseOrder | PurchaseOrderLine | `PurchaseOrderId` | Explícita |
| PurchaseOrder | PurchaseDeliveryNote | `PurchaseOrderId` | Explícita |
| PurchaseDeliveryNote | PurchaseDeliveryNoteLine | `PurchaseDeliveryNoteId` | Explícita |
| PurchaseDeliveryNote | PurchaseInvoice | `PurchaseDeliveryNoteId` | Explícita |
| PurchaseInvoice | PurchaseInvoiceLine | `PurchaseInvoiceId` | Explícita |
| Warehouse | WarehouseLocation | `WarehouseId` | Explícita |
| Warehouse | StockMovement | `WarehouseId` | Explícita |
| Warehouse | StockBalance | `WarehouseId` | Explícita |
| FiscalYear | FiscalPeriod | `FiscalYearId` | Explícita |
| FiscalPeriod | AccountingEntry | `FiscalPeriodId` | Explícita |
| AccountingJournal | AccountingEntry | `JournalId` | Explícita |
| AccountingEntry | AccountingEntryLine | `AccountingEntryId` | Explícita |
| AccountingTemplate | AccountingTemplateLine | `TemplateId` | Explícita |
| SubscriptionPlan | License | `PlanId` | Explícita |
| License | LicenseModule | `LicenseId` | Explícita |

## Relaciones N:M

| Entidad A | Entidad B | Tabla puente | Confirmación |
|-----------|-----------|--------------|--------------|
| User | Role | UserRoles | Explícita |
| Role | Permission | RolePermissions | Explícita |

## Value Objects embebidos (OwnsOne)

| Entidad | Value Object | Confirmación |
|---------|-------------|--------------|
| Customer | Address | Explícita (OwnsOne en config) |
| Supplier | SupplierAddress | Explícita (OwnsOne en config) |

## Relaciones inferidas (por nombre/uso)

| Origen | Destino | Motivo |
|--------|---------|--------|
| AccountingEntry.SourceId | SalesInvoice / PurchaseInvoice | `SourceType` string + `SourceId` GUID (polimórfico) |
| AccountingEntryLine.ThirdPartyId | Customer / Supplier | `ThirdPartyType` string |

## Soft-delete

Las siguientes entidades tienen `IsActive` (no `IsDeleted`) + `HasQueryFilter` en EF:

| Entidad | Campo |
|---------|-------|
| Customer | `IsActive` |
| Supplier | `IsActive` |
| Item | `IsActive` |
| Warehouse | `IsActive` |
| Account | `IsActive` |
