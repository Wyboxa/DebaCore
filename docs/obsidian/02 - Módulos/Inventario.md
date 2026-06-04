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

## Handlers — Commands

| Handler | Descripción |
|---------|-------------|
| `CreateWarehouseHandler` | Crea almacén |
| `AddWarehouseLocationHandler` | Añade ubicación a almacén |
| `CreateStockMovementHandler` | Registra movimiento y actualiza saldo |

## Handlers — Queries

| Handler | Descripción |
|---------|-------------|
| `GetWarehousesHandler` | Lista almacenes |
| `GetWarehouseByIdHandler` | Almacén con ubicaciones |
| `GetStockMovementsHandler` | Lista movimientos paginada |
| `GetStockMovementByIdHandler` | Movimiento por ID |
| `GetStockBalanceHandler` | Saldo por artículo/almacén |

## Controllers

| Controller | Ruta |
|------------|------|
| `WarehousesController` | `api/warehouses` |
| `StockMovementsController` | `api/stock/movements` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Almacenes.razor` | `/inventario/almacenes` | Implementada |
| `Movimientos.razor` | `/inventario/movimientos` | Implementada |
| `SaldosStock.razor` | `/inventario/saldos` | Implementada |
| `Inventario.razor` | `/inventario` | Placeholder de sección |

## Repositorios

- `IWarehouseRepository` → `WarehouseRepository`
- `IWarehouseLocationRepository` → `WarehouseLocationRepository`
- `IStockMovementRepository` → `StockMovementRepository`
- `IStockBalanceRepository` → `StockBalanceRepository`

## Seeds

`DemoDataSeeder` crea:
- Almacén `ALM-01` "Almacén Principal Sevilla"
- Stock inicial de TUB-001 (500 UN), VAL-001 (150 UN), CEM-001 (2000 KG)

## Lo que está completo

- CRUD de almacenes y ubicaciones
- Movimientos de stock con tipo (In/Out/Transfer/Adjustment)
- Saldo automático actualizado por movimiento
- UI de movimientos y saldos

## Lo que falta

- Inventario físico (`InventoryCount`)
- Ajustes de stock (`StockAdjustment`) — modelo definido en CLAUDE.md pero no en Domain
- Integración de albaranes con movimientos de stock automáticos
- Stock mínimo / alertas de rotura
