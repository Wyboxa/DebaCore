---
type: index
module: cross
layer: api
status: implemented
source:
  - src/Debales.Api/Controllers/
related:
  - Mapa API
  - 01 - Arquitectura
---

# Índice API

**Base URL**: `https://localhost:{puerto}/api/`  
**Autenticación**: JWT Bearer (header `Authorization: Bearer {token}`)  
**Todos los endpoints requieren** `[Authorize]` salvo `/api/auth/login` y `/api/health`

## Controllers

| Controller | Ruta base | Módulo |
|------------|-----------|--------|
| [[AuthController API]] | `api/auth` | Core |
| [[UsersController API]] | `api/users` | Core |
| `HealthController` | `api/health` | Core |
| [[CustomersController API]] | `api/customers` | CRM |
| [[SuppliersController API]] | `api/suppliers` | Suppliers |
| [[ItemsController API]] | `api/items` | Catalog |
| [[SalesOrdersController API]] | `api/sales/orders` | Ventas |
| `SalesDeliveryNotesController` | `api/sales/delivery-notes` | Ventas |
| `SalesInvoicesController` | `api/sales/invoices` | Ventas |
| `SalesCreditNotesController` | `api/sales/credit-notes` | Ventas |
| `CustomerPaymentsController` | `api/customers/payments` | Ventas |
| `PurchaseOrdersController` | `api/purchasing/orders` | Compras |
| `PurchaseDeliveryNotesController` | `api/purchasing/delivery-notes` | Compras |
| `PurchaseInvoicesController` | `api/purchasing/invoices` | Compras |
| `PurchaseCreditNotesController` | `api/purchasing/credit-notes` | Compras |
| `SupplierPaymentsController` | `api/supplier-payments` | Compras |
| `WarehousesController` | `api/warehouses` | Inventario |
| `StockMovementsController` | `api/stock/movements` | Inventario |
| [[AccountingController API]] | `api/accounting` | Contabilidad |
| [[AIController API]] | `api/ai` | IA |
| [[LicensesController API]] | `api/licenses` | Licenciamiento |
| `SubscriptionPlansController` | `api/subscription-plans` | Licenciamiento |

**Total: 22 controllers**
