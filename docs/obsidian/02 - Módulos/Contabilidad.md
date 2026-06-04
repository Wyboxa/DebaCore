---
type: module
module: contabilidad
layer: cross
status: implemented
source:
  - src/Debales.Domain/Accounting/
  - src/Debales.Application/Accounting/
  - src/Debales.Infrastructure/Persistence/Repositories/Accounting/
  - src/Debales.Infrastructure/Persistence/Configurations/Accounting/AccountingSeeds.cs
  - src/Debales.Api/Controllers/AccountingController.cs
  - src/Debales.Web/Components/Pages/Contabilidad/
related:
  - Account
  - FiscalYear
  - FiscalPeriod
  - AccountingJournal
  - AccountingEntry
  - AccountingEntryLine
  - AccountingTemplate
  - AccountingTemplateLine
---

# Módulo Contabilidad

## Qué problema resuelve

Plan contable PGC España, ejercicios fiscales, diarios contables, asientos manuales y automáticos (desde facturas y cobros/pagos). Motor contable mínimo.

## Estado

Implementado — migración `AddAccountingModule` (2026-06-02).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Account]] | Cuenta contable con código, nombre, tipo y flag imputabilidad |
| [[FiscalYear]] | Ejercicio fiscal con períodos |
| [[FiscalPeriod]] | Período contable (mes/trimestre) con estados Open/Closed/Locked |
| [[AccountingJournal]] | Diario contable (VTA, CPR, BCO, CAJ) |
| [[AccountingEntry]] | Asiento contable con invariante Debe = Haber |
| [[AccountingEntryLine]] | Línea de asiento (cuenta, debe/haber, tercero) |
| [[AccountingTemplate]] | Plantilla de asiento para automatización |
| [[AccountingTemplateLine]] | Línea de plantilla con tipo de importe (Percentage/Fixed) |

## Reglas de dominio (invariantes)

- `TotalDebit == TotalCredit` — validado en `AccountingEntry.Post()`
- Una línea no puede tener Debe y Haber simultáneamente — validado en `AddLine()`
- No contabilizar asientos Posted o Cancelled
- Los asientos Posted no pueden cancelarse directamente — crear asiento de reversión

## Estados de asiento

`Draft → Posted | Cancelled`

## Estados de período fiscal

`Open → Closed → Locked`

## Handlers — Commands

| Handler | Descripción |
|---------|-------------|
| `CreateAccountHandler` | Crea cuenta contable |
| `CreateFiscalYearHandler` | Crea ejercicio con períodos |
| `CloseFiscalPeriodHandler` | Cierra período fiscal |
| `CloseFiscalYearHandler` | Cierra ejercicio fiscal |
| `CreateAccountingJournalHandler` | Crea diario |
| `CreateAccountingEntryHandler` | Crea asiento en borrador |
| `PostAccountingEntryHandler` | Contabiliza asiento (valida cuadre) |

## Handlers — Queries

| Handler | Descripción |
|---------|-------------|
| `GetAccountsHandler` | Lista cuentas con búsqueda |
| `GetAccountByIdHandler` | Cuenta por ID |
| `GetFiscalYearsHandler` | Lista ejercicios |
| `GetAccountingJournalsHandler` | Lista diarios |
| `GetAccountingEntriesHandler` | Lista asientos con filtros |
| `GetAccountingEntryByIdHandler` | Asiento con líneas |

## Servicios

- `IAccountingEntryService` → `AccountingEntryService` — Generación automática de asientos desde eventos (SalesInvoicePosted, etc.)

## Controllers

| Controller | Ruta |
|------------|------|
| `AccountingController` | `api/accounting` |
| | `api/accounting/accounts` |
| | `api/accounting/fiscal-years` |
| | `api/accounting/journals` |
| | `api/accounting/entries` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `PlanContable.razor` | `/contabilidad/plan` | Implementada — lista con búsqueda y creación |
| `EjerciciosFiscales.razor` | `/contabilidad/ejercicios` | Implementada |
| `Asientos.razor` | `/contabilidad/asientos` | Implementada |

## Repositorios

- `IAccountRepository` → `AccountRepository`
- `IFiscalYearRepository` → `FiscalYearRepository`
- `IAccountingJournalRepository` → `AccountingJournalRepository`
- `IAccountingEntryRepository` → `AccountingEntryRepository`
- `IAccountingTemplateRepository` → `AccountingTemplateRepository`

## Seeds (datos de referencia PGC España)

`AccountingSeeds` define GUIDs fijos para:
- **Diarios**: VTA (Ventas), CPR (Compras), BCO (Banco), CAJ (Caja)
- **Cuentas**: 300, 400, 430, 472, 475, 477, 570, 572, 600, 621, 628, 640, 700, 705
- **Plantillas**: SalesInvoicePosted, PurchaseInvoicePosted

## Lo que está completo

- Plan contable PGC España (cuentas predefinidas)
- Ejercicios fiscales con períodos
- Diarios predefinidos
- Asientos manuales con validación de cuadre
- Plantillas de asiento para automatización
- Motor de generación automática desde facturas

## Lo que falta

- Generación automática de asientos desde cobros/pagos (Receivable/Payable liquidados)
- Cierre contable con asiento de regularización
- Informes contables (balance, cuenta de pérdidas y ganancias)
- Remesas bancarias
