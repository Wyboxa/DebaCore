---
type: ideas
module: cross
layer: cross
status: active
related:
  - Pendientes priorizados
  - Huecos funcionales
  - Decisiones arquitectónicas pendientes
---

# Ideas y decisiones del proyecto

> Este fichero es el registro persistente de ideas, propuestas y decisiones que surgen durante el desarrollo.
> Cada entrada tiene fecha y estado. Las ideas implementadas se marcan ✓.

---

## Regla: cómo usar este fichero

Cada vez que surja una idea nueva durante una sesión de desarrollo:
1. Añadirla aquí con fecha y estado `Pendiente | En análisis | Descartada | ✓ Implementada`
2. Si se implementa, moverla también a `Pendientes priorizados.md` o a `Huecos funcionales.md`
3. Si implica una decisión arquitectónica, crear ADR en `/docs/decisions/`

---

## Ideas surgidas en sesión 2026-06-05

### [✓] Presupuestos de venta (SalesQuote)
**Idea**: Completar el ciclo comercial añadiendo el eslabón de entrada: Presupuesto → Pedido → Albarán → Factura.
**Estado**: ✓ Implementado — commit `59472df`, migración `AddSalesQuoteModule`
**Notas**: Numeración `PRE-YYYY-XXXX`, estados Draft→Sent→Accepted→Convertido. `ConvertQuoteToOrderHandler` copia líneas con precios.

### [✓] Paridad Compras/Ventas en confirmación de albarán
**Idea**: `PostPurchaseDeliveryNoteHandler` debería actualizar `PurchaseOrder.Status` igual que hace ventas.
**Estado**: ✓ Implementado — commit `dd5a8c4`
**Notas**: `PurchaseOrder.UpdateReceiptStatus` y `PurchaseOrderLine.RecordReceipt` son ahora `public`.

### [✓] Informes contables
**Idea**: La contabilidad tiene datos pero no tiene forma de consultarlos como informes útiles.
**Estado**: ✓ Implementado — commit `539799f`
**Informes en `/contabilidad/informes`**:
- Balance de comprobación (sumas y saldos) — agrega por cuenta con filtro de fechas
- Libro diario — asientos Posted con líneas, ordenados por fecha
- Balance de situación — agrupa por AccountType, verifica cuadre

### [ ] AuditLog UI
**Idea**: La tabla `AuditEntries` acumula datos de auditoría pero no hay página para consultarlos.
**Estado**: Pendiente
**Propuesta**: `/configuracion/auditoria` — tabla filtrable por entidad, usuario, fecha y acción. Solo lectura.
**Valor**: Trazabilidad real de quien cambió qué y cuándo.

### [ ] ModuleRequired en páginas individuales
**Idea**: `ModuleRequired` solo está en los hubs (`/ventas`, `/compras`, etc.) pero no en las páginas de lista/detalle. Un usuario sin licencia puede navegar directamente a `/ventas/pedidos`.
**Estado**: Pendiente
**Propuesta**: Envolver el contenido de cada página de lista con `<ModuleRequired Module="Sales">`.
**Riesgo si no se hace**: El enforcement de licencia es fácilmente evitable escribiendo la URL directamente.

### [ ] Asientos automáticos desde cobros/pagos
**Idea**: `CustomerPayment` y `SupplierPayment` no generan `AccountingEntry` automático. Las facturas sí (via `AccountingTemplate`) pero los cobros/pagos no.
**Estado**: Pendiente — faltan plantillas de asiento en seeds
**Propuesta**: Añadir `AccountingTemplate` para `CustomerPaymentConfirmed` y `SupplierPaymentConfirmed` en `AccountingSeeds.cs`.
**Impacto**: La contabilidad queda incompleta sin esto — los saldos de cuentas de clientes/proveedores no se cierran.

### [ ] Series documentales configurables (InvoiceSeries)
**Idea**: Actualmente la numeración es automática (`FAC-YYYY-XXXX`) pero no configurable por serie.
**Estado**: Pendiente — baja prioridad MVP
**Propuesta**: Entidad `InvoiceSeries` con prefijo, contador y tipo de documento. Permitir al cliente configurar sus series.

### [ ] Condiciones de pago (PaymentTerm)
**Idea**: Los vencimientos se calculan con una fecha directa, pero no hay entidad `PaymentTerm` configurable (ej: "30 días", "60 días", "contado").
**Estado**: Pendiente
**Propuesta**: Entidad `PaymentTerm` con días y tipo (días netos, fin de mes, etc.). Asignable a cliente/proveedor.

### [ ] Tarifas de precio (PriceList / ItemPrice)
**Idea**: Los precios de los artículos son fijos en el catálogo. No hay tarifas por cliente ni descuentos.
**Estado**: Pendiente — media prioridad
**Propuesta**: `PriceList` → `ItemPrice` por artículo. Asignable a cliente. Al crear línea de presupuesto/pedido, buscar precio en tarifa antes de precio de catálogo.

### [ ] Recuento físico de inventario (InventoryCount)
**Idea**: Los saldos de stock se calculan por movimientos pero no hay forma de hacer un recuento físico y ajustar diferencias.
**Estado**: Pendiente — baja prioridad MVP
**Propuesta**: `InventoryCount` con líneas, diferencias y aprobación. Genera `StockAdjustment` al confirmar.

---

## Ideas anteriores (sesiones previas)

### [✓] Dashboard con datos reales (Home.razor)
**Implementado**: 2026-06-04 — KPIs reales, alertas de vencimientos, últimos pedidos y facturas.

### [✓] Gestión de usuarios desde UI
**Implementado**: 2026-06-04 — `/configuracion/usuarios` y `/configuracion/usuarios/{id}`.

### [✓] Stock automático desde albaranes
**Implementado**: 2026-06-05 — `PostSalesDeliveryNoteHandler` (Out) y `PostPurchaseDeliveryNoteHandler` (In).

### [✓] Flujo espejo Compras (albarán → factura)
**Implementado**: 2026-06-05 — `GenerateInvoiceFromPurchaseDeliveryNoteHandler` + botón en `AlbaranCompraDetalle.razor`.

### [✓] PDF export facturas
**Implementado**: 2026-06-05 — QuestPDF Community. Endpoints en `Debales.Web/Program.cs`.

### [✓] ModuleRequired (guard de licencia en hubs)
**Implementado**: 2026-06-05 — `Shared/ModuleRequired.razor` aplicado en 5 hubs.
