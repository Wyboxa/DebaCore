# Módulo Accounting — Debales

## Estado

Pendiente de implementar (Fase ERP-5)

## Dependencias

- Core (usuarios, configuración, auditoría)
- Recibe eventos de: Sales, Purchasing, Inventory

## Diseño fundamental

Ver: [ADR-0004](../decisions/ADR-0004-operational-vs-accounting-documents.md)
Ver: [ADR-0005](../decisions/ADR-0005-accounting-events.md)
Ver: [accounting-foundation.md](../architecture/accounting-foundation.md)

## Entidades

```txt
ChartOfAccounts
 ├── Code (ej: PGC2007)
 ├── Name
 └── IsDefault

Account
 ├── ChartId
 ├── Code (ej: 4300)
 ├── Name
 ├── AccountType (Asset | Liability | Equity | Revenue | Expense | Tax)
 ├── IsImputable (false = grupo, no admite apuntes directos)
 ├── IsBlocked
 └── Notes

AccountingJournal
 ├── Code (ej: VEN, COM, BAN, DG)
 ├── Name
 └── IsDefault

FiscalYear
 ├── Code (ej: 2026)
 ├── StartDate, EndDate
 ├── Status (Open | Closed | Locked)
 └── Periods: FiscalPeriod[]

FiscalPeriod
 ├── FiscalYearId
 ├── Number (1-12 o personalizado)
 ├── StartDate, EndDate
 └── Status (Open | Closed | Locked)

AccountingEntry
 ├── JournalId
 ├── FiscalPeriodId
 ├── Date
 ├── Description
 ├── Status (Draft | Posted | Cancelled | Locked)
 ├── SourceDocumentType
 ├── SourceDocumentId
 └── Lines: AccountingEntryLine[]

AccountingEntryLine
 ├── EntryId
 ├── AccountId
 ├── Debit (decimal ≥ 0)
 ├── Credit (decimal ≥ 0)
 └── Description

Receivable
 ├── InvoiceId (SalesInvoice)
 ├── CustomerId
 ├── DueDate, Amount
 ├── Status (Pending | Partial | Settled | Defaulted | Cancelled)
 └── Payments: CustomerPayment[]

CustomerPayment
 ├── CustomerId
 ├── Date, Amount, PaymentMethodId
 ├── AccountingEntryId?
 └── Allocations: PaymentAllocation[] → Receivable

Payable
 ├── InvoiceId (PurchaseInvoice)
 ├── SupplierId
 ├── DueDate, Amount
 ├── Status (Pending | Partial | Settled | Defaulted | Cancelled)
 └── Payments: SupplierPayment[]

SupplierPayment
 ├── SupplierId
 ├── Date, Amount, PaymentMethodId
 ├── AccountingEntryId?
 └── Allocations: PaymentAllocation[] → Payable

Remittance
 ├── Date, BankAccountId
 ├── Status (Draft | Sent | Confirmed | Rejected)
 └── Lines: RemittanceLine[] → Receivable
```

## Invariantes de dominio

```txt
AccountingEntry.Lines.Sum(Debit) == AccountingEntry.Lines.Sum(Credit)
AccountingEntryLine: NOT (Debit > 0 AND Credit > 0)
AccountingEntry.Status == Posted → inmutable
FiscalPeriod.Status == Closed → no nuevas entradas
Account.IsImputable == false → no puede recibir líneas de asiento
```

## Eventos consumidos

```txt
SalesInvoicePosted       → genera asiento venta + receivables
SalesInvoiceCancelled    → genera asiento de anulación
PurchaseInvoicePosted    → genera asiento compra + payables
PurchaseInvoiceCancelled → genera asiento de anulación
CustomerPaymentConfirmed → genera asiento cobro + liquida receivables
SupplierPaymentConfirmed → genera asiento pago + liquida payables
StockAdjustmentConfirmed → puede generar asiento regularización
FiscalYearClosed         → bloquea periodos del año
```
