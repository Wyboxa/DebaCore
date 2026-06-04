---
type: entity
module: inventario
layer: domain
status: implemented
source:
  - src/Debales.Domain/Inventory/Warehouse.cs
  - src/Debales.Domain/Inventory/WarehouseLocation.cs
related:
  - Inventario
  - StockMovement
  - StockBalance
---

# Warehouse (Almacén)

## Tabla EF / DbSet

`Warehouses` — `DbSet<Warehouse>`
`WarehouseLocations` — `DbSet<WarehouseLocation>`

## Propiedades de Warehouse

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Code` | `string` | Código único (uppercase) |
| `Name` | `string` | Nombre descriptivo |
| `Description` | `string?` | Descripción |
| `IsActive` | `bool` | Activo/inactivo |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(code, name, description, createdBy)` | Factory con validación |
| `Update(name, description, updatedBy)` | Actualización |
| `Deactivate(updatedBy)` | Soft-delete |
| `Activate(updatedBy)` | Re-activación |

## WarehouseLocation — propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `WarehouseId` | `Guid` | FK a Warehouse |
| `Code` | `string` | Código de ubicación |
| `Name` | `string` | Nombre |
| `Description` | `string?` | Descripción |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| WarehouseLocation (colección) | Explícita 1:N |
| StockMovement (FK WarehouseId) | Explícita |
| StockBalance (FK WarehouseId) | Explícita |

## Seed

`DemoDataSeeder` crea `ALM-01` "Almacén Principal Sevilla" con stock inicial de 3 artículos.
