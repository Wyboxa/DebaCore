---
type: database
module: cross
layer: database
status: implemented
source:
  - src/Debales.Infrastructure/Persistence/Migrations/
related:
  - DbContext
  - Seeds
---

# Migraciones EF Core

**Ensamblado de migraciones**: `Debales.Infrastructure`  
**Startup project**: `Debales.Api`

**Comando de aplicación:**
```powershell
dotnet ef database update `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj
```

La API aplica migraciones automáticamente al arrancar mediante `context.Database.MigrateAsync()`.

## Migraciones

| Orden | Nombre | Fecha | Módulo |
|-------|--------|-------|--------|
| 1 | `InitialCreate` | 2026-05-27 | Core: Users, Roles, Permissions, SystemModules, AuditEntries |
| 2 | `AddCrmModule` | 2026-05-27 | CRM: Customers, Contacts, Activities, Notes, Opportunities |
| 3 | `AddCustomerEmail` | 2026-05-28 | CRM: columna Email en Customers |
| 4 | `AddSuppliersModule` | 2026-05-28 | Suppliers: Suppliers con SupplierAddress embebida |
| 5 | `AddCatalogModule` | 2026-05-28 | Catalog: Items, ItemFamilies, UnitsOfMeasure, TaxTypes |
| 6 | `AddERP2Module` | 2026-05-29 | Sales: SalesOrders + Lines + DeliveryNotes; Purchasing: PurchaseOrders + Lines + DeliveryNotes |
| 7 | `AddERP3Module` | 2026-06-01 | Sales: Invoices, CreditNotes, Receivables, CustomerPayments; Purchasing: Invoices, CreditNotes, Payables, SupplierPayments |
| 8 | `AddERP4Module` | 2026-06-01 | Inventory: Warehouses, Locations, StockMovements, StockBalances |
| 9 | `AddAccountingModule` | 2026-06-02 | Accounting: Accounts, FiscalYears, Periods, Journals, Entries, Templates |
| 10 | `AddLicensingModule` | 2026-06-04 | Licensing: SubscriptionPlans, Licenses, LicenseModules |
| 11 | `AddSalesQuoteModule` | 2026-06-04 | Sales: SalesQuotes, SalesQuoteLines |
| 12 | `AddPaymentAccountingTemplates` | 2026-06-07 | Accounting: AccountingTemplates seed para cobros/pagos |
| 13 | `AddNumberSeriesModule` | 2026-06-07 | Core: NumberSeries |
| 14 | `AddNumberSeriesSeed` | 2026-06-09 | Seed: 9 series documentales por defecto (pendiente `database update`) |
| 15 | `AddPriceListModule` | 2026-06-09 | Catalog: PriceLists, ItemPrices, SupplierItemCodes, CustomerItemCodes |
| 16 | `AddPaymentMethodModule` | 2026-06-09 | PaymentTerm + PaymentTermLine (condiciones de pago) |
| 17 | `AddPaymentTermAndMethodSeed` | 2026-06-09 | Seed: 7 condiciones de pago + 6 formas de pago + FK en Customer/Supplier |
| 18 | `AddBankAccountModule` | 2026-06-09 | Accounting: BankAccount con FK opcional a Account |
| 19 | `AddMinimumStockToItems` | 2026-06-10 | Catalog: columna MinimumStock (decimal?) en Items |
| 20 | `AddInventoryCountModule` | 2026-06-10 | Inventory: InventoryCount + InventoryCountLine |
| 21 | `AddCashAccountModule` | 2026-06-10 | Accounting: CashAccount con FK opcional a Account |
| 22 | `AddRemittanceModule` | 2026-06-11 | Accounting: Remittance + RemittanceLine |
| 23 | `AddSupplierContactModule` | 2026-06-12 | Suppliers: SupplierContact |
| 24 | `AddDocumentsModule` | 2026-06-13 | Documents: Documents + DocumentTypes (**manual**) |
| 25 | `AddAIGovernanceModule` | 2026-06-14 | AI: AIRules, AIKnowledgeBases, AIActionProposals, AIActionApprovals, AIExecutionLogs (**manual**) |

> **Nota migraciones manuales (24-25)**: Creadas a mano por falta de .NET SDK en el equipo actual. Funcionan para `database update` pero carecen de `Designer.cs` (snapshot). Regenerar con `dotnet ef migrations remove` + `dotnet ef migrations add` tras instalar .NET 8 SDK.

## Nota sobre AddERP3Module

Esta migración también añadió la tabla de `Receivables` y `Payables` (vencimientos), que conceptualmente son de facturación pero técnicamente forman parte del mismo paquete ERP-3.

## Convención de nombres

Las migraciones se nombran con el prefijo de fecha `YYYYMMDDHHMMSS_` seguido del nombre descriptivo en PascalCase.
