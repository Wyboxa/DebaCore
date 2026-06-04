---
type: entity
module: contabilidad
layer: domain
status: implemented
source:
  - src/Debales.Domain/Accounting/Account.cs
  - src/Debales.Domain/Accounting/AccountType.cs
related:
  - Contabilidad
  - AccountingEntry
  - AccountingEntryLine
  - AccountingTemplate
---

# Account (Cuenta Contable)

## Tabla EF / DbSet

`Accounts` — `DbSet<Account>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Code` | `string` | Código PGC (ej. "430", "700", "477") |
| `Name` | `string` | Nombre descriptivo |
| `Type` | `AccountType` | Tipo: Asset, Liability, Equity, Revenue, Expense |
| `IsPostable` | `bool` | True = se pueden imputar asientos |
| `IsActive` | `bool` | Activo/inactivo |
| `ParentCode` | `string?` | Código del grupo padre |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(code, name, type, isPostable, parentCode, createdBy)` | Factory |
| `Update(name, type, isPostable, parentCode, updatedBy)` | Actualización |
| `Deactivate(updatedBy)` | Desactivación |
| `ForSeed(id, code, name, type, isPostable, parentCode)` | Factory internal para seeds |

## Cuentas PGC sembradas

| Código | Nombre (inferido) | Tipo |
|--------|-------------------|------|
| 300 | Mercaderías | Asset |
| 400 | Proveedores | Liability |
| 430 | Clientes | Asset |
| 472 | HP IVA Soportado | Asset |
| 475 | HP IVA Repercutido | Liability |
| 477 | IVA Repercutido | Liability |
| 570 | Caja | Asset |
| 572 | Bancos | Asset |
| 600 | Compras | Expense |
| 621 | Arrendamientos | Expense |
| 628 | Suministros | Expense |
| 640 | Gastos personal | Expense |
| 700 | Ventas | Revenue |
| 705 | Prestación servicios | Revenue |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| AccountingEntryLine (FK AccountId) | Explícita |
| AccountingTemplateLine (referencia AccountId) | Inferida |
