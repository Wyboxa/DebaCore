---
type: entity
module: contabilidad
layer: domain
status: implemented
source:
  - src/Debales.Domain/Accounting/AccountingEntry.cs
  - src/Debales.Domain/Accounting/AccountingEntryLine.cs
  - src/Debales.Domain/Accounting/EntryStatus.cs
related:
  - Contabilidad
  - Account
  - FiscalPeriod
  - AccountingJournal
  - SalesInvoice
  - PurchaseInvoice
---

# AccountingEntry (Asiento Contable)

## Tabla EF / DbSet

`AccountingEntries` — `DbSet<AccountingEntry>`
`AccountingEntryLines` — `DbSet<AccountingEntryLine>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Number` | `string` | Número de asiento |
| `Date` | `DateOnly` | Fecha del asiento |
| `Description` | `string` | Descripción obligatoria |
| `JournalId` | `Guid` | FK a AccountingJournal |
| `FiscalPeriodId` | `Guid` | FK a FiscalPeriod |
| `Status` | `EntryStatus` | Estado del asiento |
| `SourceType` | `string?` | Tipo de origen ("SalesInvoice", "PurchaseInvoice", etc.) |
| `SourceId` | `Guid?` | ID del documento origen |

## Propiedades calculadas

| Propiedad | Descripción |
|-----------|-------------|
| `TotalDebit` | Suma de Debe de todas las líneas |
| `TotalCredit` | Suma de Haber de todas las líneas |
| `IsBalanced` | `TotalDebit == TotalCredit` |

## Estados (EntryStatus)

`Draft → Posted | Cancelled`

## AccountingEntryLine — propiedades clave

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `AccountId` | `Guid` | FK a Account |
| `AccountCode` | `string` | Código denormalizado |
| `Description` | `string` | Descripción de la línea |
| `Debit` | `decimal` | Importe en el Debe |
| `Credit` | `decimal` | Importe en el Haber |
| `ThirdPartyId` | `Guid?` | FK a Customer o Supplier (inferido) |
| `ThirdPartyType` | `string?` | "Customer" o "Supplier" |

## Invariantes de dominio

1. `IsBalanced` — obligatorio para `Post()`. Lanza excepción si Debe ≠ Haber
2. Una línea no puede tener Debe > 0 y Haber > 0 simultáneamente
3. Los importes no pueden ser negativos
4. Solo se puede modificar un asiento Draft

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(...)` | Factory con validación de número, descripción, diario y período |
| `AddLine(accountId, accountCode, description, debit, credit, thirdPartyId, thirdPartyType)` | Añade línea (solo Draft) |
| `Post(updatedBy)` | Draft → Posted (valida cuadre y líneas) |
| `Cancel(updatedBy)` | Draft → Cancelled (Posted no cancelable directamente) |

## Relaciones

| Relación | Confirmación |
|----------|-------------|
| AccountingJournal (FK JournalId) | Explícita |
| FiscalPeriod (FK FiscalPeriodId) | Explícita |
| AccountingEntryLine | Explícita 1:N |
| SalesInvoice / PurchaseInvoice (vía SourceType/SourceId) | Inferida |
