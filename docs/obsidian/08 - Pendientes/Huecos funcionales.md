---
type: audit
module: cross
layer: cross
status: pending
related:
  - Pendientes priorizados
  - Índice de módulos
---

# Huecos funcionales

Funcionalidades previstas en CLAUDE.md §42 que no están implementadas en el dominio.

## Entidades de CLAUDE.md §42 no encontradas en código

### Ventas
- `SalesQuote` / `SalesQuoteLine` — Presupuesto de venta

### Catálogo
- `PriceList` / `ItemPrice` — Tarifas de precio por cliente
- `SupplierItemCode` — Código del artículo para el proveedor
- `CustomerItemCode` — Código del artículo para el cliente
- `Service` — Entidad separada de servicio (en código, los servicios son `Item` con `IsService=true`)

### Contabilidad
- `BankAccount` — Cuenta bancaria
- `CashAccount` — Cuenta de caja
- `Remittance` — Remesa bancaria

### Inventario
- `StockAdjustment` — Ajuste de inventario
- `InventoryCount` — Recuento físico de inventario

### IA supervisada
- `AIContext` — No implementado como entidad persistente
- `AIKnowledgeBase` — No implementado
- `AIRule` — No implementado
- `AIActionProposal` — No implementado (solo handlers en memoria)
- `AIActionApproval` — No implementado
- `AIExecutionLog` — No implementado

### Facturación
- `InvoiceSeries` — Series de facturación
- `PaymentTerm` — Condiciones de pago
- `PaymentMethod` — Formas de pago

## Integración entre módulos no implementada

| Integración | Estado |
|-------------|--------|
| Albarán venta → MovimientoStock (salida automática) | No confirmado |
| Albarán compra → MovimientoStock (entrada automática) | No confirmado |
| CustomerPayment → AccountingEntry (asiento de cobro) | No confirmado |
| SupplierPayment → AccountingEntry (asiento de pago) | No confirmado |
| Licencia → Middleware de validación de acceso | No confirmado |

## Multi-tenant

Ninguna entidad tiene campo `TenantId`. La plataforma es mono-tenant en su estado actual. CLAUDE.md §47.2 lo registra como decisión pendiente.
