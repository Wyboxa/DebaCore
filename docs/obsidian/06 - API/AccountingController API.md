---
type: api
module: contabilidad
layer: api
status: implemented
source:
  - src/Debales.Api/Controllers/AccountingController.cs
related:
  - Contabilidad
  - Account
  - FiscalYear
  - AccountingEntry
  - AccountingJournal
---

# AccountingController

**Ruta base**: `api/accounting`  
**Autorización**: JWT Bearer requerido

## Endpoints

| Método | Ruta | Handler | Descripción |
|--------|------|---------|-------------|
| GET | `/accounts` | `GetAccountsHandler` | Lista cuentas con búsqueda y paginación |
| GET | `/accounts/{id}` | `GetAccountByIdHandler` | Cuenta por ID |
| POST | `/accounts` | `CreateAccountHandler` | Crear cuenta |
| GET | `/fiscal-years` | `GetFiscalYearsHandler` | Lista ejercicios |
| POST | `/fiscal-years` | `CreateFiscalYearHandler` | Crear ejercicio |
| GET | `/journals` | `GetAccountingJournalsHandler` | Lista diarios |
| POST | `/journals` | `CreateAccountingJournalHandler` | Crear diario |
| GET | `/entries` | `GetAccountingEntriesHandler` | Lista asientos (filtros: search, journalId, fiscalPeriodId) |
| GET | `/entries/{id}` | `GetAccountingEntryByIdHandler` | Asiento con líneas |
| POST | `/entries` | `CreateAccountingEntryHandler` | Crear asiento |
| POST | `/entries/{id}/post` | `PostAccountingEntryHandler` | Contabilizar asiento |

## Request types

```csharp
record CreateAccountRequest(string Code, string Name, AccountType Type, bool IsPostable, string? ParentCode)
record CreateFiscalYearRequest(string Name, DateOnly StartDate, DateOnly EndDate)
record CreateJournalRequest(string Code, string Name)
record CreateEntryRequest(DateOnly Date, string Description, Guid JournalId, Guid FiscalPeriodId, IReadOnlyList<CreateEntryLineDto> Lines)
```

## Nota

Los endpoints de cierre de período (`CloseFiscalPeriodHandler`) y cierre de ejercicio (`CloseFiscalYearHandler`) están registrados como handlers pero **no tienen endpoint API expuesto** en este controller (no confirmado en el código del controller).
