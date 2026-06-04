---
type: entity
module: catalogo
layer: domain
status: implemented
source:
  - src/Debales.Domain/Catalog/Item.cs
related:
  - Catalogo
  - ItemFamily
  - UnitOfMeasure
  - TaxType
  - SalesOrderLine
  - PurchaseOrderLine
  - StockMovement
---

# Item (Artículo / Servicio)

## Tabla EF / DbSet

`Items` — `DbSet<Item>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Code` | `string` | Código único (uppercase) |
| `Name` | `string` | Nombre descriptivo |
| `Description` | `string?` | Descripción larga |
| `IsService` | `bool` | True = servicio, False = producto físico |
| `IsActive` | `bool` | Activo/inactivo |
| `SalePrice` | `decimal` | Precio de venta (≥ 0) |
| `PurchasePrice` | `decimal` | Precio de compra (≥ 0) |
| `FamilyId` | `Guid?` | FK a ItemFamily (opcional) |
| `UnitOfMeasureId` | `Guid` | FK a UnitOfMeasure (obligatorio) |
| `TaxTypeId` | `Guid?` | FK a TaxType (opcional) |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(code, name, description, isService, uomId, familyId, taxTypeId, salePrice, purchasePrice, createdBy)` | Factory con validación |
| `Update(...)` | Actualización completa |
| `Deactivate(updatedBy)` | Soft-delete |
| `Activate(updatedBy)` | Re-activación |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| ItemFamily (FK FamilyId) | Explícita |
| UnitOfMeasure (FK UnitOfMeasureId) | Explícita |
| TaxType (FK TaxTypeId) | Explícita |
| SalesOrderLine, PurchaseOrderLine, StockMovement (referencia ItemId) | Explícita |

## Nota de diseño

Los artículos de tipo `IsService = true` no generan movimientos de stock (inferido por lógica de negocio, no validado en la entidad).
