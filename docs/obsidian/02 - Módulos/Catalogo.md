---
type: module
module: catalogo
layer: cross
status: implemented
source:
  - src/Debales.Domain/Catalog/
  - src/Debales.Application/Catalog/
  - src/Debales.Infrastructure/Persistence/Repositories/Catalog/
  - src/Debales.Api/Controllers/ItemsController.cs
  - src/Debales.Web/Components/Pages/Catalogo/
related:
  - Item
  - ItemFamily
  - UnitOfMeasure
  - TaxType
  - DbContext
---

# Módulo Catálogo

## Qué problema resuelve

Gestión del catálogo de artículos y servicios, incluyendo familias, unidades de medida y tipos de IVA.

## Estado

Implementado — migración `AddCatalogModule` (2026-05-28). Tarifas implementadas — migración `AddPriceListModule` (2026-06-09).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Item]] | Artículo o servicio con código, precio venta/compra, IVA, familia y UoM |
| [[ItemFamily]] | Familia de artículos (PROD, SERV, etc.) |
| [[UnitOfMeasure]] | Unidad de medida (UN, KG, H, etc.) |
| [[TaxType]] | Tipo de IVA con tasa (IVA21=21%, IVA10=10%) |
| `PriceList` | Tarifa de venta con vigencia (ValidFrom/ValidTo), activa/inactiva |
| `ItemPrice` | Precio de artículo dentro de una tarifa (cascade desde PriceList) |
| `SupplierItemCode` | Código del proveedor para un artículo |
| `CustomerItemCode` | Código del cliente para un artículo |

## Handlers

| Handler | Tipo |
|---------|------|
| `CreateItemHandler` | Command |
| `UpdateItemHandler` | Command |
| `GetItemsHandler` | Query (paginado, con filtros por familia e isService) |
| `GetItemByIdHandler` | Query |
| `GetCatalogLookupsHandler` | Query (devuelve datos de referencia para dropdowns) |
| `GetPriceListsHandler` | Query (paginado, filtro por nombre/activo) |
| `GetPriceListByIdHandler` | Query (include Items→Item→UoM) |
| `CreatePriceListHandler` | Command |
| `UpdatePriceListHandler` | Command (activa/desactiva) |
| `SetItemPriceHandler` | Command (upsert de precio en tarifa) |
| `RemoveItemPriceHandler` | Command |
| `GetSupplierItemCodesHandler` | Query |
| `UpsertSupplierItemCodeHandler` | Command (crea o actualiza) |
| `DeleteSupplierItemCodeHandler` | Command |
| `GetCustomerItemCodesHandler` | Query |
| `UpsertCustomerItemCodeHandler` | Command |
| `DeleteCustomerItemCodeHandler` | Command |

## Controllers

| Controller | Ruta base |
|------------|-----------|
| `ItemsController` | `api/items` |
| `PriceListsController` | `api/pricelists` (GET/POST/PUT, sub-rutas `/items`) |
| `SuppliersController` | `api/suppliers/{id}/item-codes` (GET/POST/DELETE) |
| `CustomersController` | `api/customers/{id}/item-codes` (GET/POST/DELETE) |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Items.razor` | `/catalogo` | Implementada — lista con búsqueda, filtros y creación inline |
| `ItemDetail.razor` | `/catalogo/{id}` | Implementada — ficha con 3 tabs: Información, Movimientos (historial stock acumulado), Saldos por almacén |
| `Tarifas.razor` | `/catalogo/tarifas` | Implementada — lista con búsqueda y creación inline |
| `TarifaDetalle.razor` | `/catalogo/tarifas/{id}` | Implementada — ficha con tabla de precios (add/edit/remove) |
| `SupplierDetail.razor` (tab Códigos) | `/proveedores/{id}` → tab | Implementada |
| `CustomerDetail.razor` (tab Códigos) | `/crm/customers/{id}` → tab | Implementada |

## Repositorios

- `IItemRepository` → `ItemRepository`
- `IItemFamilyRepository` → `ItemFamilyRepository`
- `IUnitOfMeasureRepository` → `UnitOfMeasureRepository`
- `ITaxTypeRepository` → `TaxTypeRepository`
- `IPriceListRepository` → `PriceListRepository`
- `ISupplierItemCodeRepository` → `SupplierItemCodeRepository`
- `ICustomerItemCodeRepository` → `CustomerItemCodeRepository`

## Seeds

`CatalogSeeder` (y `DemoDataSeeder.EnsureCatalogAsync`) siembra:
- UoM: `UN` (Unidad), `KG` (Kilogramo), `H` (Hora)
- TaxTypes: `IVA21` (21%), `IVA10` (10%)
- ItemFamilies: `PROD` (Productos), `SERV` (Servicios)

## Lo que está completo

- CRUD de artículos con flag `IsService`
- Precios de venta y compra
- Búsqueda y filtro por tipo y familia desde UI
- Datos de referencia para dropdowns en pedidos/albaranes/facturas
- Tarifas de precio (`PriceList` + `ItemPrice`) — CRUD completo con UI
- Códigos de artículo por proveedor y cliente — upsert/delete con UI en fichas
- `MinimumStock` en `Item` — campo nullable con alerta "Bajo mínimo" en SaldosStock y Dashboard
- `ItemDetail.razor` con 3 tabs: Información, Movimientos (historial con saldo acumulado), Saldos por almacén
- `GetItemPriceHandler` con 6 tests: resolución desde tarifa, fallback a precio venta, tarifa sin entrada para el artículo, tarifa no encontrada

## Lo que falta

- (sin pendientes de primer nivel)
