---
type: index
module: cross
layer: domain
status: implemented
related:
  - 00 - Inicio
  - 01 - Arquitectura
---

# Índice de entidades de dominio

Todas las entidades encontradas en `src/Debales.Domain/`.

## Core

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[User]] | Entity | `Core/Users/User.cs` |
| [[Role]] | Entity | `Core/Roles/Role.cs` |
| [[Permission]] | Entity | `Core/Roles/Permission.cs` |
| [[RolePermission]] | Entity | `Core/Roles/RolePermission.cs` |
| [[UserRole]] | Entity | `Core/Users/UserRole.cs` |
| [[AuditEntry]] | Entity | `Core/Audit/AuditEntry.cs` |
| [[SystemModule]] | Entity | `Core/Modules/SystemModule.cs` |
| Email | Value Object | `Core/Users/Email.cs` |

## CRM

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[Customer]] | Entity | `CRM/Customers/Customer.cs` |
| [[Address]] | Value Object | `CRM/Customers/Address.cs` |
| [[Contact]] | Entity | `CRM/Contacts/Contact.cs` |
| [[Activity]] | Entity | `CRM/Activities/Activity.cs` |
| [[Note]] | Entity | `CRM/Notes/Note.cs` |
| [[Opportunity]] | Entity | `CRM/Opportunities/Opportunity.cs` |

## Suppliers

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[Supplier]] | Entity | `Suppliers/Supplier.cs` |
| [[SupplierAddress]] | Value Object | `Suppliers/SupplierAddress.cs` |

## Catalog

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[Item]] | Entity | `Catalog/Item.cs` |
| [[ItemFamily]] | Entity | `Catalog/ItemFamily.cs` |
| [[UnitOfMeasure]] | Entity | `Catalog/UnitOfMeasure.cs` |
| [[TaxType]] | Entity | `Catalog/TaxType.cs` |

## Sales

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[SalesOrder]] | Entity | `Sales/SalesOrder.cs` |
| [[SalesOrderLine]] | Entity | `Sales/SalesOrderLine.cs` |
| [[SalesDeliveryNote]] | Entity | `Sales/SalesDeliveryNote.cs` |
| [[SalesDeliveryNoteLine]] | Entity | `Sales/SalesDeliveryNoteLine.cs` |
| [[SalesInvoice]] | Entity | `Sales/SalesInvoice.cs` |
| [[SalesInvoiceLine]] | Entity | `Sales/SalesInvoiceLine.cs` |
| [[SalesCreditNote]] | Entity | `Sales/SalesCreditNote.cs` |
| [[SalesCreditNoteLine]] | Entity | `Sales/SalesCreditNoteLine.cs` |
| [[Receivable]] | Entity | `Sales/Receivable.cs` |
| [[CustomerPayment]] | Entity | `Sales/CustomerPayment.cs` |

## Purchasing

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[PurchaseOrder]] | Entity | `Purchasing/PurchaseOrder.cs` |
| [[PurchaseOrderLine]] | Entity | `Purchasing/PurchaseOrderLine.cs` |
| [[PurchaseDeliveryNote]] | Entity | `Purchasing/PurchaseDeliveryNote.cs` |
| [[PurchaseDeliveryNoteLine]] | Entity | `Purchasing/PurchaseDeliveryNoteLine.cs` |
| [[PurchaseInvoice]] | Entity | `Purchasing/PurchaseInvoice.cs` |
| [[PurchaseInvoiceLine]] | Entity | `Purchasing/PurchaseInvoiceLine.cs` |
| [[PurchaseCreditNote]] | Entity | `Purchasing/PurchaseCreditNote.cs` |
| [[PurchaseCreditNoteLine]] | Entity | `Purchasing/PurchaseCreditNoteLine.cs` |
| [[Payable]] | Entity | `Purchasing/Payable.cs` |
| [[SupplierPayment]] | Entity | `Purchasing/SupplierPayment.cs` |

## Inventory

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[Warehouse]] | Entity | `Inventory/Warehouse.cs` |
| [[WarehouseLocation]] | Entity | `Inventory/WarehouseLocation.cs` |
| [[StockMovement]] | Entity | `Inventory/StockMovement.cs` |
| [[StockBalance]] | Entity | `Inventory/StockBalance.cs` |

## Accounting

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[Account]] | Entity | `Accounting/Account.cs` |
| [[FiscalYear]] | Entity | `Accounting/FiscalYear.cs` |
| [[FiscalPeriod]] | Entity | `Accounting/FiscalPeriod.cs` |
| [[AccountingJournal]] | Entity | `Accounting/AccountingJournal.cs` |
| [[AccountingEntry]] | Entity | `Accounting/AccountingEntry.cs` |
| [[AccountingEntryLine]] | Entity | `Accounting/AccountingEntryLine.cs` |
| [[AccountingTemplate]] | Entity | `Accounting/AccountingTemplate.cs` |
| [[AccountingTemplateLine]] | Entity | `Accounting/AccountingTemplateLine.cs` |

## Licensing

| Entidad | Tipo | Archivo |
|---------|------|---------|
| [[License]] | Entity | `Licensing/License.cs` |
| [[LicenseModule]] | Entity | `Licensing/LicenseModule.cs` |
| [[SubscriptionPlan]] | Entity | `Licensing/SubscriptionPlan.cs` |

**Total: 51 entidades/value objects de dominio**
