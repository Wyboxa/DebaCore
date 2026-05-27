# Fundamentos de Contabilidad — Debales

## Principio fundamental

La contabilidad es una capa separada del negocio operativo.

```txt
Negocio operativo  →  publica eventos  →  módulo contable  →  asientos
```

Ver: [ADR-0004](../decisions/ADR-0004-operational-vs-accounting-documents.md)
Ver: [ADR-0005](../decisions/ADR-0005-accounting-events.md)

## Modelo de datos contable mínimo

```txt
FiscalYear
 └── FiscalPeriod (estado: Open | Closed | Locked)

ChartOfAccounts
 └── Account
       ├── Code (ej: 4300)
       ├── Name (ej: "Clientes, euros")
       ├── AccountType (Asset | Liability | Equity | Revenue | Expense)
       ├── IsImputable (false = cuenta de grupo, no admite apuntes directos)
       └── IsBlocked

AccountingJournal (ej: Ventas, Compras, Bancos, Diario General)

AccountingEntry
 ├── JournalId
 ├── FiscalPeriodId
 ├── Date
 ├── Description
 ├── Status (Draft | Posted | Cancelled | Locked)
 ├── SourceDocumentType (SalesInvoice | CustomerPayment | …)
 ├── SourceDocumentId
 └── Lines: AccountingEntryLine[]
       ├── AccountId
       ├── Debit (decimal)
       ├── Credit (decimal)
       └── Description
```

### Invariante del asiento

```txt
Sum(Lines.Debit) == Sum(Lines.Credit)
```

Este invariante se valida en el dominio al pasar de `Draft` a `Posted`.
Un asiento descuadrado no puede validarse.

## Flujo de contabilización

```txt
1. Evento operativo publicado (ej: SalesInvoicePosted)
2. AccountingModule recibe el evento
3. Determina plantilla de asiento para ese tipo de evento
4. Construye AccountingEntry en estado Draft
5. Valida invariante (Debe == Haber)
6. Transiciona a Posted si validación OK
7. Registra relación entre asiento y documento origen
8. Si validación falla → excepción, sin asiento creado
```

## Cierre de periodo

```txt
FiscalPeriod.Close()
 → marca periodo como Closed
 → no permite nuevos asientos en ese periodo
 → no afecta asientos existentes en Draft de otros periodos

FiscalPeriod.Lock()
 → marca periodo como Locked
 → ninguna modificación es posible
 → requiere aprobación explícita para reabrir
```

## Vencimientos y tesorería

```txt
Receivable (vencimiento de cobro)
 ├── InvoiceId
 ├── DueDate
 ├── Amount
 ├── Status (Pending | Partial | Settled | Defaulted | Cancelled)
 └── Payments: CustomerPayment[]

Payable (vencimiento de pago)
 ├── InvoiceId
 ├── DueDate
 ├── Amount
 ├── Status (Pending | Partial | Settled | Defaulted | Cancelled)
 └── Payments: SupplierPayment[]
```

Un cobro o pago puede liquidar uno o varios vencimientos (cobro parcial, pago agrupado).

## Reglas no negociables

Ver detalle completo en `CLAUDE.md §43.8`.

Resumen:
- No contabilizar en ejercicio/periodo cerrado.
- No asientos descuadrados.
- No líneas con Debe y Haber simultáneos.
- No imputar en cuentas de grupo (IsImputable = false).
- No modificar asientos Posted o Locked.
- Toda anulación es trazable.
- No borrar: anular o rectificar.
