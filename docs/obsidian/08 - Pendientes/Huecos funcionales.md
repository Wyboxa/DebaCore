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
| CustomerPayment → AccountingEntry (asiento de cobro) | ✓ Implementado — AccountingTemplate seed (2026-06-07) | 2026-06-07 |
| SupplierPayment → AccountingEntry (asiento de pago) | ✓ Implementado — AccountingTemplate seed (2026-06-07) | 2026-06-07 |
| Licencia → guard en hubs de UI (ModuleRequired) | ✓ Implementado | 2026-06-05 |
| Licencia → guard en páginas de lista/detalle | ✓ Implementado — `ModuleRouteGuard` en `MainLayout` | 2026-06-09 |

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
| `AIKnowledgeBase` | ✓ Implementado — módulo AIGovernance (2026-06-14) |
| `AIRule` | ✓ Implementado — módulo AIGovernance (2026-06-14) |
| `AIActionProposal` / `AIActionApproval` / `AIExecutionLog` | ✓ Implementado — módulo AIGovernance (2026-06-14) |

### Documents
| Entidad | Estado |
|---------|--------|
| `Document` | ✓ Implementado — módulo Documents (2026-06-13) |
| `DocumentType` | ✓ Implementado — módulo Documents (2026-06-13) |
| `DocumentVersion` | Pendiente |
| `DocumentTemplate` | Pendiente |
| `DocumentAttachment` (subida real de ficheros) | Pendiente |

### Facturación
| Entidad | Estado |
|---------|--------|
| `NumberSeries` | ✓ Implementado — series documentales configurables (2026-06-07/09) |
| `PaymentTerm` | ✓ Implementado — AddPaymentMethodModule (2026-06-09) |
| `PaymentMethod` | ✓ Implementado — AddPaymentTermAndMethodSeed (2026-06-09) |

### Contabilidad ampliada
| Entidad | Estado |
|---------|--------|
| `BankAccount` | ✓ Implementado (2026-06-09) |
| `CashAccount` | ✓ Implementado (2026-06-10) |
| `Remittance` / `RemittanceLine` | ✓ Implementado (2026-06-11) |

### Catálogo ampliado
| Entidad | Estado |
|---------|--------|
| `PriceList` / `ItemPrice` | ✓ Implementado (2026-06-09) |
| `SupplierItemCode` / `CustomerItemCode` | ✓ Implementado (2026-06-09) |
| `Item.MinimumStock` | ✓ Implementado (2026-06-10) |

### Inventario ampliado
| Entidad | Estado |
|---------|--------|
| `InventoryCount` / `InventoryCountLine` | ✓ Implementado (2026-06-10) |

## Multi-tenant
`TenantId` (Guid?) añadido a `AuditableEntity` — se auto-asigna en `SaveChangesAsync` vía `ITenantService`. Migración `20260614160000_AddTenantIdToBusinessEntities` añade la columna a ~50 tablas. En producción mono-tenant el campo queda en NULL (sin impacto funcional).

## Subida de archivos
`IFileStorageService` + `LocalFileStorageService` — guarda en `uploads/` relativo al `AppContext.BaseDirectory`. Path configurable con `FileStorage:BasePath`. Endpoint `POST /api/documents/{id}/upload` y UI en `DocumentoDetalle.razor`.

## IA → Propuesta
Botón "Guardar como propuesta IA" en Chat ERP — captura el último intercambio user/assistant y lo persiste como `AIActionProposal` con `ActionType = "ChatAnalysis"`. Visible en `/ai/propuestas` para revisión humana.
