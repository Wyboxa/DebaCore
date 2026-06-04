---
type: audit
module: cross
layer: cross
status: not_confirmed
related:
  - Contradicciones detectadas
  - Huecos funcionales
  - Deuda técnica
  - Mejoras UI
---

# Pendientes priorizados

> Última actualización: 2026-06-05

## Resueltos en sesión 2026-06-05

| Item | Estado |
|------|--------|
| P1 — Actualizar CLAUDE.md (Licensing, Docker como completos) | ✓ Resuelto — commit `f434a9f` |
| P3 — Validación de licencia en middleware | ✓ Resuelto — `ModuleRequired.razor` en hub pages |
| P4 — Integración almacén con albaranes | ✓ Resuelto — `PostSalesDeliveryNoteHandler` crea movimientos Out; `PostPurchaseDeliveryNoteHandler` crea In |
| P5 — Gestión de usuarios desde UI | ✓ Resuelto — `/configuracion/usuarios` + `/configuracion/usuarios/{id}` |
| P10 — Dashboard analítico | ✓ Resuelto — `Home.razor` con 6 KPIs reales, alertas, pedidos e facturas recientes |
| Flujo espejo Compras (Generar albarán → Generar factura) | ✓ Resuelto — `AlbaranCompraDetalle.razor` con selector de almacén + botón generar factura |
| PDF export facturas | ✓ Resuelto — QuestPDF en `InvoicePdfGenerator`, endpoints `/descargar/factura-{venta,compra}/{id}` |
| Configuración con datos reales | ✓ Resuelto — `Configuracion.razor` muestra versión, usuarios activos, roles, estado de licencia |

---

## Prioridad 1 — Presupuestos de venta (SalesQuote)

**Qué falta**: La entidad `SalesQuote` / `SalesQuoteLine` está en el modelo conceptual (CLAUDE.md §42.5) pero no existe en el dominio, base de datos ni UI.

**Impacto**: El ciclo comercial está incompleto — Presupuesto → Pedido → Albarán → Factura. Sin presupuesto, el primer paso documentado es el pedido.

**Estimación**: Media. Patron idéntico a SalesOrder.

---

## Prioridad 2 — Integración asientos contables desde cobros/pagos

**Qué falta**: Cuando se registra un `CustomerPayment` o `SupplierPayment`, no se confirma que se genere asiento contable automático.

Las plantillas de asiento (`AccountingTemplate`) están definidas para `SalesInvoicePosted` y `PurchaseInvoicePosted`, pero no hay plantillas para cobros/pagos en `AccountingSeeds`.

**Impacto**: La contabilidad queda incompleta — los cobros y pagos no tienen reflejo contable automático.

---

## Prioridad 3 — Informes contables

**Qué falta**: No hay endpoints ni páginas para:
- Balance de situación
- Cuenta de pérdidas y ganancias
- Balance de comprobación (sumas y saldos)
- Libro diario

**Impacto**: Sin informes, la contabilidad es solo entrada de datos.

---

## Prioridad 4 — ModuleRequired en páginas de lista (no solo hubs)

**Qué falta**: `ModuleRequired` está aplicado en los hubs (`/ventas`, `/compras`, `/inventario`, `/facturacion`, `/analitica`), pero no en las páginas de lista y detalle individuales (`/ventas/pedidos`, `/contabilidad/asientos`, etc.).

**Impacto**: Un usuario sin módulo Sales puede navegar directamente a `/ventas/pedidos` sin bloqueo.

**Solución**: Envolver `@body` en cada página de lista/detalle con `<ModuleRequired Module="Sales">`.

---

## Prioridad 5 — Actualización de estado de PurchaseOrder al confirmar albarán

**Qué falta**: `PostPurchaseDeliveryNoteHandler` no actualiza `PurchaseOrder.Status` a `Delivered` cuando se confirma el albarán de compra, a diferencia del handler de ventas que sí actualiza `SalesOrder.Status`.

**Impacto**: El pedido de compra queda en estado `Pending` aunque el albarán esté confirmado.

---

## Prioridad 6 — Tarifas de precio y códigos de artículo por tercero

**Qué falta**: Las entidades `PriceList`, `ItemPrice`, `SupplierItemCode`, `CustomerItemCode` del CLAUDE.md §42.4 no están implementadas.

---

## Prioridad 7 — AuditLog UI

**Qué falta**: La tabla `AuditEntries` existe en la base de datos y se escribe, pero no hay página Blazor para consultarla.

**Impacto**: La auditoría es invisible para el usuario.

---

## Prioridad 8 — Tests para AssignRoleHandler y DeactivateUserHandler

**Qué falta**: Los handlers `AssignRoleHandler` y `DeactivateUserHandler` no tienen tests unitarios todavía.

---

## Prioridad 9 — Importación masiva de datos

**Qué falta**: No hay funcionalidad de importación CSV/Excel para clientes, proveedores, artículos.

---

## Prioridad 10 — Multi-tenant

**Qué falta**: Decisión arquitectónica pendiente (ver CLAUDE.md §47.2). Si se decide multi-tenant, todas las tablas necesitan `TenantId`.

**Impacto estratégico alto** — afecta a toda la base de datos y lógica de acceso.
