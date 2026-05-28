# Roadmap — Debales

> Generado: 2026-05-28  
> Basado en: CLAUDE.md §30, §46, §47 + auditoría de código  
> Estado de partida: **Fases 0–3 completadas** (solución .NET 8, Core, CRM, API, UI Blazor, 31 tests)

---

## Prioridad 0 — Requisitos críticos (antes de cualquier nueva funcionalidad)

Estas tareas bloquean el uso real de la aplicación y deben resolverse antes de avanzar en nuevos módulos.

### P0-1 · Autenticación y autorización

- Implementar autenticación en `Debales.Api` (JWT o cookie — decidir según despliegue objetivo).
- Añadir `[Authorize]` a todos los controllers existentes.
- Crear pantalla de login en `Debales.Web`.
- Gestionar sesión de usuario en Blazor (AuthenticationStateProvider).
- Exponer `ClaimsPrincipal` en handlers para resolver `CreatedBy` real.

**Por qué es P0:** sin autenticación, la aplicación no puede mostrarse ni en demo.

### P0-2 · Seed de datos iniciales

- Crear `DbInitializer` o `IHostedService` de seed.
- Datos mínimos: usuario admin, roles base (Admin, User), módulos Core y CRM registrados en `SystemModules`.

**Por qué es P0:** sin seed, la aplicación arranca vacía e inutilizable.

### P0-3 · Middleware de errores global

- Añadir `UseExceptionHandler` o middleware custom en `Debales.Api`.
- Respuesta de error consistente (Problem Details — RFC 7807).

### P0-4 · Corregir bug de autorización en oportunidades

- Validar que `opportunityId` pertenece al `customerId` indicado en la ruta antes de ejecutar `UpdateOpportunityStatus`.

### P0-5 · Endpoint GET notas

- Añadir `GET /api/customers/{id}/notes` al `CustomersController`.

---

## Fase 4 — IA documental (siguiente fase técnica oficial)

**Objetivo:** IA útil sin tocar código. Chat con contexto del CRM, resumen de cliente, consulta de documentación interna.

### Tareas

- Decidir proveedor IA para desarrollo (Claude API, OpenAI o mock avanzado).
- Implementar `ClaudeProvider` o `OpenAIProvider` en `Debales.AI`.
- Registrar proveedor IA en DI con configuración por entorno.
- Crear `AIContextBuilder` — construye el contexto de un cliente (datos CRM + permisos).
- Crear endpoint o servicio de chat: `POST /api/ai/chat` con contexto controlado.
- Añadir pestaña "Asistente IA" en la ficha de cliente (Blazor).
- Resumen automático de cliente (últimas actividades, oportunidades abiertas, notas recientes).
- Registro de conversaciones IA en `AIExecutionLog`.

**Dependencias:** P0-1 (necesita usuario autenticado para construir el contexto IA).

---

## Fase ERP-1 — Proveedores y catálogo base

**Objetivo:** infraestructura base para el ERP. Permite gestionar lo que se compra y vende.

### Módulo Supplier

- Entidades: `Supplier`, `SupplierContact`, `SupplierAddress`
- CRUD completo (domain, application, infrastructure, API, UI)
- Tests de dominio y application

### Módulo Catalog

- Entidades: `Item`, `Service`, `ItemFamily`, `UnitOfMeasure`, `TaxType`
- `PriceList` con `ItemPrice`
- `SupplierItemCode`, `CustomerItemCode`
- CRUD completo
- Tests

### Infraestructura compartida

- `NumberSeries` (series documentales: FAC, PED, ALB…)
- `PaymentTerm`, `PaymentMethod`

**Dependencias:** Core y CRM completados. ✅

---

## Fase ERP-2 — Ventas y compras básicas

**Objetivo:** ciclo operativo sin contabilizar. Pedidos y albaranes.

### Módulo Sales (operativo)

- `SalesQuote` + `SalesQuoteLine` (presupuesto)
- `SalesOrder` + `SalesOrderLine` (pedido cliente)
- `SalesDeliveryNote` + `SalesDeliveryNoteLine` (albarán de venta)
- Flujo: presupuesto → pedido → albarán

### Módulo Purchasing (operativo)

- `PurchaseOrder` + `PurchaseOrderLine`
- `PurchaseDeliveryNote` + `PurchaseDeliveryNoteLine`
- Flujo: pedido → albarán de compra

**Dependencias:** Fase ERP-1.

---

## Fase ERP-3 — Facturación

**Objetivo:** generar facturas y gestionar cobros/pagos sin contabilizar.

- `SalesInvoice` + `SalesInvoiceLine` (desde albaranes o directa)
- `SalesCreditNote` (rectificativa de venta)
- `Receivable` (vencimiento de cobro)
- `CustomerPayment` (cobro)
- `PurchaseInvoice` + `PurchaseInvoiceLine`
- `PurchaseCreditNote`
- `Payable` (vencimiento de pago)
- `SupplierPayment` (pago)
- `InvoiceSeries` (series de facturación con numeración automática)

**Dependencias:** Fase ERP-2. Requiere decisión sobre formas de pago y SEPA (§47.2).

---

## Fase ERP-4 — Almacén básico

**Objetivo:** control de stock.

- `Warehouse`, `WarehouseLocation`
- `StockMovement` (desde albaranes de venta y compra)
- `StockBalance` (saldo por artículo/almacén)
- `StockAdjustment`, `InventoryCount`

**Dependencias:** Fase ERP-2.

---

## Fase ERP-5 — Contabilidad mínima

**Objetivo:** contabilidad básica generada automáticamente desde eventos operativos.

- Resolver decisiones pendientes del §47.2:
  - Motor contable (implementación propia vs librería)
  - Plan contable por defecto (PGC España)
  - Cuentas individuales por cliente/proveedor
  - Plantillas de asientos vs reglas hardcoded
- `ChartOfAccounts`, `Account`
- `FiscalYear`, `FiscalPeriod`
- `AccountingJournal`
- `AccountingEntry`, `AccountingEntryLine`
- Asientos automáticos desde: `SalesInvoicePosted`, `PurchaseInvoicePosted`, `CustomerPaymentConfirmed`, `SupplierPaymentConfirmed`
- Cierre de periodo y ejercicio
- Invariante garantizado en dominio: `TotalDebe == TotalHaber`

**Dependencias:** Fase ERP-3. Requiere ADR-0004 y ADR-0005 aprobados (ya existen en docs). Requiere decisiones §47.2.

---

## Fase 5 — IA técnica supervisada

**Objetivo:** IA como copiloto técnico controlado.

- Generación de planes técnicos desde requisitos
- Generación de ramas y preparación de código
- Generación de tests
- Pull requests supervisados con revisión humana
- `AIActionProposal`, `AIActionApproval` en BD
- Flujo completo: propuesta → revisión → aprobación → implementación

**Dependencias:** Fase 4 (IA documental operativa).

---

## Fase 6 — Licenciamiento

**Objetivo:** base para el modelo comercial.

- `License`, `LicenseModule`, `LicenseFeature`
- `SubscriptionPlan`
- Validación de módulos contratados en runtime
- Modo offline con expiración controlada
- Bloqueo gradual (no destructivo)

---

## Fase 7 — Despliegue local / on-premise

**Objetivo:** instalación realista en empresa cliente.

- Docker Compose con SQL Server
- Configuración por cliente (`.env` / secrets)
- Scripts de backup y restore
- Mecanismo de actualización controlado
- Documentación de instalación

---

## Fase ERP-6 — IA supervisada sobre ERP

**Objetivo:** IA con contexto de ventas, compras y contabilidad.

- IA con acceso a documentos ERP del cliente
- Propuestas de asientos contables
- Detección de anomalías en facturación
- Resúmenes de deuda/crédito por cliente y proveedor
- Aprobación humana antes de contabilizar

**Dependencias:** Fase ERP-5 + Fase 5.

---

## Decisiones pendientes que desbloquean fases

Antes de implementar los elementos marcados, estas decisiones deben resolverse como ADR en `/docs/decisions`:

| Decisión | Bloquea | Estado |
|----------|---------|--------|
| Multi-tenant desde inicio o diferir | Todas las tablas futuras | Pendiente |
| Motor contable propio vs librería | Fase ERP-5 | Pendiente |
| Plan contable por defecto | Fase ERP-5 | Pendiente |
| Formas de pago y remesas (SEPA) | Fase ERP-3 | Pendiente |
| Generación automática de asientos | Fase ERP-5 | Pendiente |
| Cuentas contables individuales por tercero | Fase ERP-5 | Pendiente |
| Proveedor IA para desarrollo | Fase 4 | Pendiente |
| Esquema de autenticación (JWT vs cookie) | P0-1 | Pendiente |

---

## Resumen visual de fases

```
[P0] Autenticación + Seed + Errores       ← AHORA (crítico)
[F4] IA Documental                        ← Siguiente fase técnica
[ERP-1] Proveedores + Catálogo
[ERP-2] Ventas + Compras (operativo)
[ERP-3] Facturación
[ERP-4] Almacén
[ERP-5] Contabilidad mínima              ← Requiere decisiones §47.2
[F5]  IA Técnica Supervisada
[F6]  Licenciamiento
[F7]  Despliegue local
[ERP-6] IA sobre ERP
```
