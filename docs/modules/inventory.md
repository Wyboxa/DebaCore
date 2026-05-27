# Módulo Inventory — Debales

## Estado

Pendiente de implementar (Fase ERP-4)

## Dependencias

- Core (configuración, auditoría)
- Catalog (artículos — solo los que tienen ManagesStock = true)

## Entidades

```txt
Warehouse
 ├── Code, Name
 ├── IsDefault
 └── IsActive

WarehouseLocation
 ├── WarehouseId
 ├── Code, Name (ej: A-01-01)
 └── IsActive

StockMovement
 ├── ItemId
 ├── WarehouseId, LocationId?
 ├── MovementType (Entry | Exit | Transfer | Adjustment)
 ├── Quantity (positivo siempre; el tipo indica dirección)
 ├── Date
 ├── Reference (ej: número de albarán)
 ├── SourceDocumentType, SourceDocumentId
 └── Notes

StockBalance
 ├── ItemId
 ├── WarehouseId
 └── Quantity (calculado o mantenido como proyección)

StockAdjustment
 ├── Date, Reason
 ├── Status (Draft | Confirmed | Cancelled)
 └── Lines: StockAdjustmentLine[]
       ├── ItemId, WarehouseId
       ├── TheoreticalQuantity (lo que debería haber)
       ├── ActualQuantity (lo que hay)
       └── Difference (calculado)

InventoryCount
 ├── Date, Status (Open | Counting | Closed)
 └── Lines: InventoryCountLine[]
```

## Reglas de negocio

- El stock no puede ser negativo salvo configuración explícita que lo permita.
- Un `StockMovement` nunca se borra; si hay error, se genera movimiento inverso.
- El `StockBalance` puede ser calculado (suma de movimientos) o mantenido como proyección actualizada.
- Cada movimiento tiene referencia al documento que lo originó.
- Un ajuste de inventario confirmado genera movimientos de stock y puede generar asiento contable.

## Eventos publicados

```txt
StockMovementCreated(itemId, warehouseId, quantity, type)
StockAdjustmentConfirmed(adjustmentId, lines)
InventoryCountClosed(countId, differences)
```
