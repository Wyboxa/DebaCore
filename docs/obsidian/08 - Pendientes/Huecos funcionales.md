---
type: audit
module: cross
layer: cross
status: partial
related:
  - Pendientes priorizados
  - Índice de módulos
---

# Huecos funcionales

Funcionalidades previstas en CLAUDE.md §42 y su estado real en el código.

> Última revisión: 2026-06-05

## Integración entre módulos

| Integración | Estado | Fecha |
|-------------|--------|-------|
| Albarán venta → StockMovement (salida automática) | ✓ Implementado | 2026-06-05 |
| Albarán compra → StockMovement (entrada automática) | ✓ Implementado | 2026-06-05 |
| Albarán compra → actualiza estado PurchaseOrder | ✓ Implementado | 2026-06-05 |
| SalesInvoice → AccountingEntry automático | ✓ Implementado (AccountingTemplate) | 2026-06-02 |
| PurchaseInvoice → AccountingEntry automático | ✓ Implementado (AccountingTemplate) | 2026-06-02 |
| CustomerPayment → AccountingEntry (asiento de cobro) | Pendiente — sin plantilla en seeds | — |
| SupplierPayment → AccountingEntry (asiento de pago) | Pendiente — sin plantilla en seeds | — |
| Licencia → guard en hubs de UI (ModuleRequired) | ✓ Implementado | 2026-06-05 |
| Licencia → guard en páginas de lista/detalle | Pendiente | — |

## Entidades de CLAUDE.md §42 — estado

### Ventas
| Entidad | Estado |
|---------|--------|
| `SalesQuote` / `SalesQuoteLine` | ✓ Implementado — `AddSalesQuoteModule` |
| `SalesOrder` / `SalesOrderLine` | ✓ Implementado |
| `SalesDeliveryNote` / Lines | ✓ Implementado |
| `SalesInvoice` / Lines | ✓ Implementado |
| `SalesCreditNote` / Lines | ✓ Implementado |
| `Receivable` | ✓ Implementado |
| `CustomerPayment` | ✓ Implementado |

### Catálogo
| Entidad | Estado |
|---------|--------|
| `Item` | ✓ Implementado |
| `ItemFamily` | ✓ Implementado |
| `UnitOfMeasure` | ✓ Implementado |
| `TaxType` | ✓ Implementado |
| `PriceList` / `ItemPrice` | Pendiente — tarifas por cliente |
| `SupplierItemCode` | Pendiente |
| `CustomerItemCode` | Pendiente |
| `Service` | Los servicios son `Item` con flag — no entidad separada (decisión deliberada) |

### Contabilidad
| Entidad | Estado |
|---------|--------|
| `Account`, `FiscalYear`, `FiscalPeriod` | ✓ Implementado |
| `AccountingJournal`, `AccountingEntry`, `AccountingEntryLine` | ✓ Implementado |
| `AccountingTemplate` / `AccountingTemplateLine` | ✓ Implementado |
| `BankAccount` | Pendiente |
| `CashAccount` | Pendiente |
| `Remittance` | Pendiente — fuera de MVP |

### Inventario
| Entidad | Estado |
|---------|--------|
| `Warehouse`, `WarehouseLocation` | ✓ Implementado |
| `StockMovement`, `StockBalance` | ✓ Implementado |
| `StockAdjustment` | Pendiente |
| `InventoryCount` | Pendiente — recuento físico |

### IA supervisada
| Entidad | Estado |
|---------|--------|
| `AIContext` (en memoria) | Implementado como contexto ERP efímero |
| `AIKnowledgeBase` | Pendiente — no persistente |
| `AIRule` | Pendiente |
| `AIActionProposal` / `AIActionApproval` / `AIExecutionLog` | Pendiente — flujo supervisado no persistido |

### Facturación
| Entidad | Estado |
|---------|--------|
| `InvoiceSeries` | Pendiente — se usa numeración automática sin series configurables |
| `PaymentTerm` | Pendiente |
| `PaymentMethod` | Pendiente |

## Multi-tenant
Ninguna entidad tiene campo `TenantId`. La plataforma es mono-tenant en su estado actual. Decisión pendiente en CLAUDE.md §47.2.
