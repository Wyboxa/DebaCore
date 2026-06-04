---
type: audit
module: cross
layer: cross
status: implemented
related:
  - 00 - Inicio
  - Cobertura de documentación
---

# Inventario técnico completo

Generado mediante auditoría del repositorio en 2026-06-04.

## Proyectos

| Nombre | Tipo | Estado |
|--------|------|--------|
| Debales.Domain | Class Library .NET 8 | Implementado |
| Debales.Application | Class Library .NET 8 | Implementado |
| Debales.Infrastructure | Class Library .NET 8 | Implementado |
| Debales.AI | Class Library .NET 8 | Implementado |
| Debales.Api | Web API .NET 8 | Implementado |
| Debales.Web | Blazor Server .NET 8 | Implementado |
| Debales.Domain.Tests | xUnit | Implementado |
| Debales.Application.Tests | xUnit | Implementado |
| Debales.Integration.Tests | xUnit | Implementado |

## Entidades de dominio (51)

| Nombre | Módulo | Archivo | Estado |
|--------|--------|---------|--------|
| User | Core | Core/Users/User.cs | implemented |
| Role | Core | Core/Roles/Role.cs | implemented |
| Permission | Core | Core/Roles/Permission.cs | implemented |
| RolePermission | Core | Core/Roles/RolePermission.cs | implemented |
| UserRole | Core | Core/Users/UserRole.cs | implemented |
| AuditEntry | Core | Core/Audit/AuditEntry.cs | implemented |
| SystemModule | Core | Core/Modules/SystemModule.cs | implemented |
| Email (VO) | Core | Core/Users/Email.cs | implemented |
| Customer | CRM | CRM/Customers/Customer.cs | implemented |
| Address (VO) | CRM | CRM/Customers/Address.cs | implemented |
| Contact | CRM | CRM/Contacts/Contact.cs | implemented |
| Activity | CRM | CRM/Activities/Activity.cs | implemented |
| Note | CRM | CRM/Notes/Note.cs | implemented |
| Opportunity | CRM | CRM/Opportunities/Opportunity.cs | implemented |
| Supplier | Suppliers | Suppliers/Supplier.cs | implemented |
| SupplierAddress (VO) | Suppliers | Suppliers/SupplierAddress.cs | implemented |
| Item | Catalog | Catalog/Item.cs | implemented |
| ItemFamily | Catalog | Catalog/ItemFamily.cs | implemented |
| UnitOfMeasure | Catalog | Catalog/UnitOfMeasure.cs | implemented |
| TaxType | Catalog | Catalog/TaxType.cs | implemented |
| SalesOrder | Sales | Sales/SalesOrder.cs | implemented |
| SalesOrderLine | Sales | Sales/SalesOrderLine.cs | implemented |
| SalesDeliveryNote | Sales | Sales/SalesDeliveryNote.cs | implemented |
| SalesDeliveryNoteLine | Sales | Sales/SalesDeliveryNoteLine.cs | implemented |
| SalesInvoice | Sales | Sales/SalesInvoice.cs | implemented |
| SalesInvoiceLine | Sales | Sales/SalesInvoiceLine.cs | implemented |
| SalesCreditNote | Sales | Sales/SalesCreditNote.cs | implemented |
| SalesCreditNoteLine | Sales | Sales/SalesCreditNoteLine.cs | implemented |
| Receivable | Sales | Sales/Receivable.cs | implemented |
| CustomerPayment | Sales | Sales/CustomerPayment.cs | implemented |
| PurchaseOrder | Purchasing | Purchasing/PurchaseOrder.cs | implemented |
| PurchaseOrderLine | Purchasing | Purchasing/PurchaseOrderLine.cs | implemented |
| PurchaseDeliveryNote | Purchasing | Purchasing/PurchaseDeliveryNote.cs | implemented |
| PurchaseDeliveryNoteLine | Purchasing | Purchasing/PurchaseDeliveryNoteLine.cs | implemented |
| PurchaseInvoice | Purchasing | Purchasing/PurchaseInvoice.cs | implemented |
| PurchaseInvoiceLine | Purchasing | Purchasing/PurchaseInvoiceLine.cs | implemented |
| PurchaseCreditNote | Purchasing | Purchasing/PurchaseCreditNote.cs | implemented |
| PurchaseCreditNoteLine | Purchasing | Purchasing/PurchaseCreditNoteLine.cs | implemented |
| Payable | Purchasing | Purchasing/Payable.cs | implemented |
| SupplierPayment | Purchasing | Purchasing/SupplierPayment.cs | implemented |
| Warehouse | Inventory | Inventory/Warehouse.cs | implemented |
| WarehouseLocation | Inventory | Inventory/WarehouseLocation.cs | implemented |
| StockMovement | Inventory | Inventory/StockMovement.cs | implemented |
| StockBalance | Inventory | Inventory/StockBalance.cs | implemented |
| Account | Accounting | Accounting/Account.cs | implemented |
| FiscalYear | Accounting | Accounting/FiscalYear.cs | implemented |
| FiscalPeriod | Accounting | Accounting/FiscalPeriod.cs | implemented |
| AccountingJournal | Accounting | Accounting/AccountingJournal.cs | implemented |
| AccountingEntry | Accounting | Accounting/AccountingEntry.cs | implemented |
| AccountingEntryLine | Accounting | Accounting/AccountingEntryLine.cs | implemented |
| AccountingTemplate | Accounting | Accounting/AccountingTemplate.cs | implemented |
| AccountingTemplateLine | Accounting | Accounting/AccountingTemplateLine.cs | implemented |
| License | Licensing | Licensing/License.cs | implemented |
| LicenseModule | Licensing | Licensing/LicenseModule.cs | implemented |
| SubscriptionPlan | Licensing | Licensing/SubscriptionPlan.cs | implemented |

## Controllers API (22)

| Nombre | Ruta | Módulo | Estado |
|--------|------|--------|--------|
| AuthController | api/auth | Core | implemented |
| UsersController | api/users | Core | implemented |
| HealthController | api/health | Core | implemented |
| CustomersController | api/customers | CRM | implemented |
| SuppliersController | api/suppliers | Suppliers | implemented |
| ItemsController | api/items | Catalog | implemented |
| SalesOrdersController | api/sales/orders | Ventas | implemented |
| SalesDeliveryNotesController | api/sales/delivery-notes | Ventas | implemented |
| SalesInvoicesController | api/sales/invoices | Ventas | implemented |
| SalesCreditNotesController | api/sales/credit-notes | Ventas | implemented |
| CustomerPaymentsController | api/customers/payments | Ventas | implemented |
| PurchaseOrdersController | api/purchasing/orders | Compras | implemented |
| PurchaseDeliveryNotesController | api/purchasing/delivery-notes | Compras | implemented |
| PurchaseInvoicesController | api/purchasing/invoices | Compras | implemented |
| PurchaseCreditNotesController | api/purchasing/credit-notes | Compras | implemented |
| SupplierPaymentsController | api/supplier-payments | Compras | implemented |
| WarehousesController | api/warehouses | Inventario | implemented |
| StockMovementsController | api/stock/movements | Inventario | implemented |
| AccountingController | api/accounting | Contabilidad | implemented |
| AIController | api/ai | IA | implemented |
| LicensesController | api/licenses | Licenciamiento | implemented |
| SubscriptionPlansController | api/subscription-plans | Licenciamiento | implemented |

## Páginas Blazor (44)

Ver [[Rutas Blazor]] para el mapa completo de rutas.

## Migraciones (10)

Ver [[Migraciones EF Core]] para detalle.

## Repositorios (30+)

| Repositorio | Implementación | Módulo |
|-------------|---------------|--------|
| IUserRepository | UserRepository | Core |
| IItemRepository | ItemRepository | Catalog |
| IItemFamilyRepository | ItemFamilyRepository | Catalog |
| IUnitOfMeasureRepository | UnitOfMeasureRepository | Catalog |
| ITaxTypeRepository | TaxTypeRepository | Catalog |
| ICustomerRepository | CustomerRepository | CRM |
| ISupplierRepository | SupplierRepository | Suppliers |
| IContactRepository | ContactRepository | CRM |
| IActivityRepository | ActivityRepository | CRM |
| INoteRepository | NoteRepository | CRM |
| IOpportunityRepository | OpportunityRepository | CRM |
| ISalesOrderRepository | SalesOrderRepository | Ventas |
| ISalesDeliveryNoteRepository | SalesDeliveryNoteRepository | Ventas |
| ISalesInvoiceRepository | SalesInvoiceRepository | Ventas |
| ISalesCreditNoteRepository | SalesCreditNoteRepository | Ventas |
| IReceivableRepository | ReceivableRepository | Ventas |
| ICustomerPaymentRepository | CustomerPaymentRepository | Ventas |
| IPurchaseOrderRepository | PurchaseOrderRepository | Compras |
| IPurchaseDeliveryNoteRepository | PurchaseDeliveryNoteRepository | Compras |
| IPurchaseInvoiceRepository | PurchaseInvoiceRepository | Compras |
| IPurchaseCreditNoteRepository | PurchaseCreditNoteRepository | Compras |
| IPayableRepository | PayableRepository | Compras |
| ISupplierPaymentRepository | SupplierPaymentRepository | Compras |
| IWarehouseRepository | WarehouseRepository | Inventario |
| IWarehouseLocationRepository | WarehouseLocationRepository | Inventario |
| IStockMovementRepository | StockMovementRepository | Inventario |
| IStockBalanceRepository | StockBalanceRepository | Inventario |
| IAccountRepository | AccountRepository | Contabilidad |
| IFiscalYearRepository | FiscalYearRepository | Contabilidad |
| IAccountingJournalRepository | AccountingJournalRepository | Contabilidad |
| IAccountingEntryRepository | AccountingEntryRepository | Contabilidad |
| IAccountingTemplateRepository | AccountingTemplateRepository | Contabilidad |
| ILicenseRepository | LicenseRepository | Licenciamiento |
| ISubscriptionPlanRepository | SubscriptionPlanRepository | Licenciamiento |
