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

Implementado — migración `AddCatalogModule` (2026-05-28).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Item]] | Artículo o servicio con código, precio venta/compra, IVA, familia y UoM |
| [[ItemFamily]] | Familia de artículos (PROD, SERV, etc.) |
| [[UnitOfMeasure]] | Unidad de medida (UN, KG, H, etc.) |
| [[TaxType]] | Tipo de IVA con tasa (IVA21=21%, IVA10=10%) |

## Handlers

| Handler | Tipo |
|---------|------|
| `CreateItemHandler` | Command |
| `UpdateItemHandler` | Command |
| `GetItemsHandler` | Query (paginado, con filtros por familia e isService) |
| `GetItemByIdHandler` | Query |
| `GetCatalogLookupsHandler` | Query (devuelve datos de referencia para dropdowns) |

## Controllers

| Controller | Ruta base |
|------------|-----------|
| `ItemsController` | `api/items` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Items.razor` | `/catalogo` | Implementada — lista con búsqueda, filtros y creación inline |
| `ItemDetail.razor` | `/catalogo/{id}` | Implementada — ficha de artículo |

## Repositorios

- `IItemRepository` → `ItemRepository`
- `IItemFamilyRepository` → `ItemFamilyRepository`
- `IUnitOfMeasureRepository` → `UnitOfMeasureRepository`
- `ITaxTypeRepository` → `TaxTypeRepository`

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

## Lo que falta

- Tarifas de precio por cliente (`PriceList`, `ItemPrice`)
- Códigos de artículo por proveedor/cliente (`SupplierItemCode`, `CustomerItemCode`)
- Control de stock mínimo y máximo
