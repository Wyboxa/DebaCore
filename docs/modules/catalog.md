# Módulo Catalog — Debales

## Estado

Pendiente de implementar (Fase ERP-1)

## Dependencias

- Core (configuración, auditoría)

## Entidades

```txt
ItemFamily
 ├── Code, Name
 └── ParentFamilyId? (jerarquía de familias)

UnitOfMeasure
 ├── Code (ej: UN, KG, ML, H)
 └── Name

TaxType
 ├── Code (ej: IVA21, IVA10, IVA0, EXENTO)
 ├── Rate (decimal, ej: 0.21)
 └── IsDefault

Item
 ├── Code, Name, Description
 ├── FamilyId
 ├── UnitOfMeasureId
 ├── DefaultTaxTypeId
 ├── IsActive
 ├── IsService (false = artículo físico, true = servicio)
 ├── ManagesStock (solo si !IsService)
 └── Notes

PriceList
 ├── Code, Name
 ├── Currency (ISO 4217, ej: EUR)
 ├── ValidFrom, ValidTo?
 └── IsDefault

ItemPrice
 ├── ItemId
 ├── PriceListId
 ├── UnitPrice
 └── MinimumQuantity?

SupplierItemCode
 ├── ItemId
 ├── SupplierId
 ├── SupplierCode
 └── SupplierDescription?

CustomerItemCode
 ├── ItemId
 ├── CustomerId
 └── CustomerCode
```

## Reglas de negocio

- Un artículo puede tener precio en varias tarifas.
- El precio aplicado en un documento se resuelve por tarifa del cliente, luego tarifa por defecto.
- Los artículos de tipo servicio no generan movimientos de stock.
- Cambiar el precio de tarifa no afecta documentos ya creados.
- El tipo de IVA por defecto del artículo puede sobreescribirse en el documento.

## Notas de implementación

El catálogo es compartido entre los módulos Sales, Purchasing e Inventory.
Ninguno de esos módulos modifica entidades del catálogo: solo las consultan.
