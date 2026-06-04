---
type: database
module: cross
layer: database
status: implemented
source:
  - src/Debales.Infrastructure/Persistence/ApplicationDbContext.cs
related:
  - Mapa de tablas
  - Migraciones EF Core
  - 01 - Arquitectura
---

# ApplicationDbContext

**Namespace**: `Debales.Infrastructure.Persistence`  
**Archivo**: `src/Debales.Infrastructure/Persistence/ApplicationDbContext.cs`

Configuraciones EF: `modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)` — aplica todas las `IEntityTypeConfiguration<T>` del ensamblado automáticamente.

## DbSets por módulo

### Core

| DbSet | Tipo |
|-------|------|
| `Users` | `DbSet<User>` |
| `Roles` | `DbSet<Role>` |
| `Permissions` | `DbSet<Permission>` |
| `UserRoles` | `DbSet<UserRole>` |
| `RolePermissions` | `DbSet<RolePermission>` |
| `SystemModules` | `DbSet<SystemModule>` |
| `AuditEntries` | `DbSet<AuditEntry>` |

### CRM

| DbSet | Tipo |
|-------|------|
| `Customers` | `DbSet<Customer>` |
| `Contacts` | `DbSet<Contact>` |
| `Activities` | `DbSet<Activity>` |
| `Notes` | `DbSet<Note>` |
| `Opportunities` | `DbSet<Opportunity>` |

### Suppliers

| DbSet | Tipo |
|-------|------|
| `Suppliers` | `DbSet<Supplier>` |

### Catalog

| DbSet | Tipo |
|-------|------|
| `Items` | `DbSet<Item>` |
| `ItemFamilies` | `DbSet<ItemFamily>` |
| `UnitsOfMeasure` | `DbSet<UnitOfMeasure>` |
| `TaxTypes` | `DbSet<TaxType>` |

### Sales (ERP-2)

| DbSet | Tipo |
|-------|------|
| `SalesOrders` | `DbSet<SalesOrder>` |
| `SalesOrderLines` | `DbSet<SalesOrderLine>` |
| `SalesDeliveryNotes` | `DbSet<SalesDeliveryNote>` |
| `SalesDeliveryNoteLines` | `DbSet<SalesDeliveryNoteLine>` |

### Sales (ERP-3)

| DbSet | Tipo |
|-------|------|
| `SalesInvoices` | `DbSet<SalesInvoice>` |
| `SalesInvoiceLines` | `DbSet<SalesInvoiceLine>` |
| `SalesCreditNotes` | `DbSet<SalesCreditNote>` |
| `SalesCreditNoteLines` | `DbSet<SalesCreditNoteLine>` |
| `Receivables` | `DbSet<Receivable>` |
| `CustomerPayments` | `DbSet<CustomerPayment>` |

### Purchasing (ERP-2 + ERP-3)

| DbSet | Tipo |
|-------|------|
| `PurchaseOrders` | `DbSet<PurchaseOrder>` |
| `PurchaseOrderLines` | `DbSet<PurchaseOrderLine>` |
| `PurchaseDeliveryNotes` | `DbSet<PurchaseDeliveryNote>` |
| `PurchaseDeliveryNoteLines` | `DbSet<PurchaseDeliveryNoteLine>` |
| `PurchaseInvoices` | `DbSet<PurchaseInvoice>` |
| `PurchaseInvoiceLines` | `DbSet<PurchaseInvoiceLine>` |
| `PurchaseCreditNotes` | `DbSet<PurchaseCreditNote>` |
| `PurchaseCreditNoteLines` | `DbSet<PurchaseCreditNoteLine>` |
| `Payables` | `DbSet<Payable>` |
| `SupplierPayments` | `DbSet<SupplierPayment>` |

### Inventory (ERP-4)

| DbSet | Tipo |
|-------|------|
| `Warehouses` | `DbSet<Warehouse>` |
| `WarehouseLocations` | `DbSet<WarehouseLocation>` |
| `StockMovements` | `DbSet<StockMovement>` |
| `StockBalances` | `DbSet<StockBalance>` |

### Accounting (ERP-5)

| DbSet | Tipo |
|-------|------|
| `Accounts` | `DbSet<Account>` |
| `FiscalYears` | `DbSet<FiscalYear>` |
| `FiscalPeriods` | `DbSet<FiscalPeriod>` |
| `AccountingJournals` | `DbSet<AccountingJournal>` |
| `AccountingEntries` | `DbSet<AccountingEntry>` |
| `AccountingEntryLines` | `DbSet<AccountingEntryLine>` |
| `AccountingTemplates` | `DbSet<AccountingTemplate>` |
| `AccountingTemplateLines` | `DbSet<AccountingTemplateLine>` |

### Licensing (Fase 6)

| DbSet | Tipo |
|-------|------|
| `SubscriptionPlans` | `DbSet<SubscriptionPlan>` |
| `Licenses` | `DbSet<License>` |
| `LicenseModules` | `DbSet<LicenseModule>` |

**Total: 48 DbSets**
