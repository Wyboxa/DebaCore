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
  - BankAccount
  - CashAccount
  - Remittance
  - RemittanceLine
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

- `IAccountingEntryService` → `AccountingEntryService` — Generación automática de asientos desde eventos:
  - `GenerateFromSalesInvoiceAsync` — evento `SalesInvoicePosted`
  - `GenerateFromPurchaseInvoiceAsync` — evento `PurchaseInvoicePosted`
  - `GenerateFromCustomerPaymentAsync` — evento `CustomerPaymentConfirmed`
  - `GenerateFromSupplierPaymentAsync` — evento `SupplierPaymentConfirmed`

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
- **Plantillas**: SalesInvoicePosted, PurchaseInvoicePosted, CustomerPaymentConfirmed, SupplierPaymentConfirmed
- Migración `AddPaymentAccountingTemplates` (2026-06-07) — ciclo contable completo operativo

## Lo que está completo

- Plan contable PGC España (cuentas predefinidas)
- Ejercicios fiscales con períodos
- Diarios predefinidos
- Asientos manuales con validación de cuadre
- Plantillas de asiento para automatización
- Motor de generación automática desde facturas
- `BankAccount` — cuentas bancarias con IBAN, BIC, vinculación a cuenta contable (2026-06-10)
- `CashAccount` — cajas con código, nombre, moneda, saldo actual, vinculación a cuenta contable (2026-06-10)

## CashAccount

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Code` | string | Código único de la caja |
| `Name` | string | Nombre descriptivo |
| `CurrencyCode` | string | Moneda (default EUR) |
| `CurrentBalance` | decimal | Saldo actual |
| `AccountId` | Guid? | FK opcional a Account |
| `IsActive` | bool | Activa/inactiva |

### Handlers CashAccount

| Handler | Descripción |
|---------|-------------|
| `CreateCashAccountHandler` | Crea caja, valida código único |
| `UpdateCashAccountHandler` | Actualiza, contiene `ToDto()` reutilizado |
| `DeleteCashAccountHandler` | Soft-delete |
| `GetCashAccountsHandler` | Lista con búsqueda y filtro IsActive |
| `GetCashAccountByIdHandler` | Detalle con Account incluido |

### Páginas Blazor CashAccount

| Página | Ruta |
|--------|------|
| `CajasCuentas.razor` | `/contabilidad/cajas` |

## Remesas (`Remittance` + `RemittanceLine`)

Módulo completo implementado 2026-06-11.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Number` | string | Numeración automática REM-C-YYYY-NNNN / REM-P-YYYY-NNNN |
| `Date` | DateOnly | Fecha de la remesa |
| `BankAccountId` | Guid | Cuenta bancaria asociada |
| `Type` | `RemittanceType` | Collection (cobros) / Payment (pagos) |
| `Status` | `RemittanceStatus` | Draft → Sent → Confirmed / Failed |
| `Notes` | string? | Observaciones |
| `FailReason` | string? | Motivo si falla |

### Máquina de estados

`Draft → Sent` (requiere líneas) → `Confirmed` (liquida vencimientos) / `Failed`

### RemittanceLine

- `DocumentId` — FK a `Receivable.Id` (Collection) o `Payable.Id` (Payment)
- `Amount` — importe incluido en la remesa
- Índice único `(RemittanceId, DocumentId)` — no duplicados

### Handlers Remittance

| Handler | Descripción |
|---------|-------------|
| `CreateRemittanceHandler` | Crea remesa, genera número automático |
| `UpdateRemittanceHandler` | Actualiza notas (solo Draft) |
| `DeleteRemittanceHandler` | Soft-delete (bloquea Confirmed) |
| `AddRemittanceLineHandler` | Añade vencimiento a la remesa |
| `RemoveRemittanceLineHandler` | Quita vencimiento |
| `SendRemittanceHandler` | Draft → Sent |
| `ConfirmRemittanceHandler` | Sent → Confirmed + aplica `ApplyPayment()` en vencimientos |
| `FailRemittanceHandler` | Sent → Failed |
| `GetRemittancesHandler` | Lista con filtros tipo/estado |
| `GetRemittanceByIdHandler` | Detalle con líneas y BankAccount |

### Páginas Blazor

| Página | Ruta |
|--------|------|
| `Remesas.razor` | `/contabilidad/remesas` — lista + modal crear + filtros |
| `RemesaDetalle.razor` | `/contabilidad/remesas/{id}` — ficha + añadir/quitar líneas + transiciones |

### API

| Endpoint | Método | Descripción |
|----------|--------|-------------|
| `api/remittances` | GET | Lista con filtros |
| `api/remittances/{id}` | GET | Detalle |
| `api/remittances` | POST | Crear |
| `api/remittances/{id}` | PUT | Editar notas |
| `api/remittances/{id}` | DELETE | Eliminar |
| `api/remittances/{id}/lines` | POST | Añadir línea |
| `api/remittances/{id}/lines/{documentId}` | DELETE | Quitar línea |
| `api/remittances/{id}/send` | POST | Enviar |
| `api/remittances/{id}/confirm` | POST | Confirmar |
| `api/remittances/{id}/fail` | POST | Marcar fallida |

## Informe de vencimientos aging

Implementado 2026-06-11. `AgingReportDto` en `Debales.Application.Common`.

Buckets: **Corriente** (sin vencer) / **1-30** / **31-60** / **61-90** / **+90** días.

`DaysOverdue = Math.Max(0, today.DayNumber - dueDate.DayNumber)` — los no vencidos van al bucket Corriente.

| Handler | Repositorio | Descripción |
|---------|-------------|-------------|
| `GetReceivablesAgingHandler` | `IReceivableRepository.GetForAgingAsync` | Cobros pendientes/parciales por antigüedad |
| `GetPayablesAgingHandler` | `IPayableRepository.GetForAgingAsync` | Pagos pendientes/parciales por antigüedad |

### API — ReportsController

| Endpoint | Método | Descripción |
|----------|--------|-------------|
| `api/reports/receivables-aging` | GET | Informe aging cobros (`?customerId=`) |
| `api/reports/payables-aging` | GET | Informe aging pagos (`?supplierId=`) |
| `api/reports/treasury-position` | GET | Posición de tesorería |

### Páginas Blazor

| Página | Ruta |
|--------|------|
| `Vencimientos.razor` | `/contabilidad/vencimientos` — 2 tabs (Cobros/Pagos), 5 bucket cards, tabla coloreada |

## Posición de tesorería

Implementado 2026-06-11.

`GetTreasuryPositionHandler` — lee todos los `BankAccount` + `CashAccount` activos y suma saldos.

`TreasuryPositionDto` en `AccountingDtos`: `(AsOf, TotalBankBalance, TotalCashBalance, TotalBalance, BankAccounts, CashAccounts)`.

> Nota: `BankAccount` no tiene campo `Balance` en dominio — muestra 0 hasta que se implemente conciliación bancaria. `CashAccount.CurrentBalance` se usa directamente.

### Páginas Blazor

| Página | Ruta |
|--------|------|
| `Tesoreria.razor` | `/contabilidad/tesoreria` — 3 KPIs + tabla bancos + tabla cajas |

El Dashboard (`Home.razor`) incluye una card de tesorería con link a `/contabilidad/tesoreria`.

## Estado de cuenta cliente/proveedor

Implementado 2026-06-11. `StatementDto` + `StatementLineDto` en `Debales.Application.Common`.

Muestra todos los movimientos de un tercero (facturas, rectificativas, cobros/pagos) ordenados por fecha con **saldo acumulado progresivo**.

| Handler | Descripción |
|---------|-------------|
| `GetCustomerStatementHandler` | Facturas + rectificativas venta + cobros de un cliente |
| `GetSupplierStatementHandler` | Facturas + rectificativas compra + pagos de un proveedor |

Lógica de saldo:
- **Cliente**: `Debit = factura`, `Credit = rectificativa + cobro`, saldo = Σ(Debit - Credit)
- **Proveedor**: `Debit = rectificativa + pago`, `Credit = factura`, saldo = Σ(Credit - Debit)

Cada repositorio tiene nuevo método `GetByCustomerForStatementAsync` / `GetBySupplierForStatementAsync` con filtro por rango de fechas.

### API

| Endpoint | Método | Descripción |
|----------|--------|-------------|
| `api/reports/customer-statement/{customerId}` | GET | Estado de cuenta cliente (`?from=&to=`) |
| `api/reports/supplier-statement/{supplierId}` | GET | Estado de cuenta proveedor (`?from=&to=`) |

### Páginas Blazor

| Página | Ruta |
|--------|------|
| `EstadoCuentaClientes.razor` | `/contabilidad/estado-cuenta-clientes` — selector de cliente + rango fechas |
| `EstadoCuentaProveedores.razor` | `/contabilidad/estado-cuenta-proveedores` — selector de proveedor + rango fechas |
| Tab "Estado de cuenta" | `CustomerDetail.razor` — tab integrado en ficha de cliente |
| Tab "Estado de cuenta" | `SupplierDetail.razor` — tab integrado en ficha de proveedor |

## Lo que falta

- Cierre contable con asiento de regularización
- Conciliación bancaria (balance real en BankAccount)
