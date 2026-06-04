---
type: entity
module: crm
layer: domain
status: implemented
source:
  - src/Debales.Domain/Suppliers/Supplier.cs
  - src/Debales.Domain/Suppliers/SupplierAddress.cs
related:
  - Suppliers
  - SupplierAddress
  - PurchaseOrder
  - PurchaseInvoice
---

# Supplier (Proveedor)

## Tabla EF / DbSet

`Suppliers` — `DbSet<Supplier>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Name` | `string` | Nombre del proveedor (obligatorio) |
| `TaxId` | `string?` | NIF/CIF |
| `Phone` | `string?` | Teléfono |
| `Email` | `string?` | Email (lowercase) |
| `Website` | `string?` | Sitio web |
| `ContactName` | `string?` | Nombre del contacto principal |
| `Notes` | `string?` | Notas libres |
| `IsActive` | `bool` | Activo/inactivo |
| `Address` | `SupplierAddress?` | Value object embebido (`OwnsOne`) |
| `AccountCode` | `string?` | Código contable para integración |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(name, taxId, phone, email, contactName, createdBy)` | Factory con validación |
| `Update(name, taxId, phone, email, website, contactName, notes, updatedBy)` | Actualización completa |
| `SetAddress(address, updatedBy)` | Asigna dirección embebida |
| `Deactivate(updatedBy)` | Soft-delete (IsActive = false) |
| `SetAccountCode(accountCode, updatedBy)` | Vincula código contable |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| SupplierAddress (embebida via OwnsOne) | Explícita |
| PurchaseOrder (FK SupplierId en PurchaseOrder) | Explícita |
| PurchaseInvoice (FK SupplierId) | Explícita |
