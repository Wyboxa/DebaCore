---
type: module
module: inventario
layer: cross
status: implemented
source:
  - src/Debales.Domain/Inventory/
  - src/Debales.Application/Inventory/
  - src/Debales.Infrastructure/Persistence/Repositories/Inventory/
  - src/Debales.Api/Controllers/WarehousesController.cs
  - src/Debales.Api/Controllers/StockMovementsController.cs
  - src/Debales.Web/Components/Pages/Inventario/
related:
  - Warehouse
  - WarehouseLocation
  - StockMovement
  - StockBalance
  - Item
---

# Módulo Inventario

## Qué problema resuelve

Gestión del almacén: almacenes, ubicaciones, movimientos de entrada/salida y saldos de stock por artículo y almacén.

## Estado

Implementado — migración `AddERP4Module` (2026-06-01).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Warehouse]] | Almacén con código, nombre y ubicaciones |
| [[WarehouseLocation]] | Ubicación dentro de un almacén |
| [[StockMovement]] | Movimiento de stock (In, Out, Transfer, Adjustment) |
| [[StockBalance]] | Saldo actual de un artículo en un almacén |
| [[InventoryCount]] | Sesión de recuento físico con líneas por artículo |
| [[InventoryCountLine]] | Línea de recuento: stock sistema vs. cantidad contada |

## Handlers — Commands

| Handler | Descripción |
|---------|-------------|
| `CreateWarehouseHandler` | Crea almacén |
| `AddWarehouseLocationHandler` | Añade ubicación a almacén |
| `CreateStockMovementHandler` | Registra movimiento y actualiza saldo |
| `AdjustStockHandler` | Ajusta stock a cantidad objetivo (calcula delta → Adjustment movement) |
| `CreateInventoryCountHandler` | Crea sesión de recuento para un almacén |
| `AddInventoryCountLineHandler` | Añade artículo al recuento con su stock sistema actual |
| `SetCountedQuantityHandler` | Registra la cantidad contada para un artículo |
| `CloseInventoryCountHandler` | Cierra recuento y genera movimientos de ajuste automáticos |

## Handlers — Queries

| Handler | Descripción |
|---------|-------------|
| `GetWarehousesHandler` | Lista almacenes |
| `GetWarehouseByIdHandler` | Almacén con ubicaciones |
| `GetStockMovementsHandler` | Lista movimientos paginada |
| `GetStockMovementByIdHandler` | Movimiento por ID |
| `GetStockBalanceHandler` | Saldo por artículo/almacén |
| `GetInventoryCountsHandler` | Lista recuentos paginada |
| `GetInventoryCountByIdHandler` | Recuento con todas sus líneas |

## Controllers

| Controller | Ruta |
|------------|------|
| `WarehousesController` | `api/warehouses` |
| `StockMovementsController` | `api/stock/movements` |
| `InventoryCountsController` | `api/inventorycounts` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Almacenes.razor` | `/inventario/almacenes` | Implementada |
| `Movimientos.razor` | `/inventario/movimientos` | Implementada |
| `SaldosStock.razor` | `/inventario/saldos` | Implementada — botón Ajustar + badge Bajo mínimo |
| `ConteoInventario.razor` | `/inventario/conteo` | Implementada — lista de recuentos + crear |
| `ConteoInventarioDetalle.razor` | `/inventario/conteo/{id}` | Implementada — añadir artículos, contar, cerrar |
| `Inventario.razor` | `/inventario` | Placeholder de sección |

## Repositorios

- `IWarehouseRepository` → `WarehouseRepository`
- `IWarehouseLocationRepository` → `WarehouseLocationRepository`
- `IStockMovementRepository` → `StockMovementRepository`
- `IStockBalanceRepository` → `StockBalanceRepository`
- `IInventoryCountRepository` → `InventoryCountRepository`

## Seeds

`DemoDataSeeder` crea:
- Almacén `ALM-01` "Almacén Principal Sevilla"
- Stock inicial de TUB-001 (500 UN), VAL-001 (150 UN), CEM-001 (2000 KG)

## Lo que está completo

- CRUD de almacenes y ubicaciones
- Movimientos de stock con tipo (In/Out/Transfer/Adjustment)
- Saldo automático actualizado por movimiento
- UI de movimientos y saldos
- Ajuste de stock desde UI de Saldos (modal, calcula delta, genera Adjustment movement)
- `Item.MinimumStock`: campo decimal opcional, visible en ficha y editable; badge "Bajo mínimo" en saldos; alerta en dashboard
- `InventoryCount` completo: Domain + Application + Infrastructure + API + UI lista + detalle
  - Flujo: crear sesión → añadir artículos → introducir cantidades contadas → cerrar (genera ajustes automáticos)
  - Migración: `AddInventoryCountModule` (2026-06-10)

## Lo que falta

- `InventoryCount` exportable a PDF
- Multi-almacén en el mismo recuento
