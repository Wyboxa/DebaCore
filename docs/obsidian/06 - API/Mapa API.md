---
type: api
module: cross
layer: api
status: implemented
source:
  - src/Debales.Api/Controllers/
related:
  - Índice API
  - 01 - Arquitectura
---

# Mapa completo de endpoints API

## Core

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| POST | `/api/auth/login` | Login, devuelve JWT | No |
| POST | `/api/users` | Crear usuario | Sí |
| GET | `/api/users/{id}` | Obtener usuario | Sí |
| GET | `/api/health` | Health check | No |

## CRM

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/customers` | Lista clientes (paginada, search) |
| GET | `/api/customers/{id}` | Obtener cliente |
| POST | `/api/customers` | Crear cliente |
| PUT | `/api/customers/{id}` | Actualizar cliente |
| POST | `/api/customers/{id}/contacts` | Añadir contacto |
| GET | `/api/customers/{id}/contacts` | Listar contactos |
| POST | `/api/customers/{id}/activities` | Registrar actividad |
| GET | `/api/customers/{id}/activities` | Listar actividades |
| POST | `/api/customers/{id}/notes` | Añadir nota |
| GET | `/api/customers/{id}/notes` | Listar notas |
| POST | `/api/customers/{id}/opportunities` | Crear oportunidad |
| GET | `/api/customers/{id}/opportunities` | Listar oportunidades |

## Suppliers

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/suppliers` | Lista proveedores |
| GET | `/api/suppliers/{id}` | Obtener proveedor |
| POST | `/api/suppliers` | Crear proveedor |
| PUT | `/api/suppliers/{id}` | Actualizar proveedor |

## Catalog

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/items` | Lista artículos (paginada, filtros) |
| GET | `/api/items/{id}` | Obtener artículo |
| POST | `/api/items` | Crear artículo |
| PUT | `/api/items/{id}` | Actualizar artículo |
| GET | `/api/items/lookups` | Datos de referencia (UoM, TaxType, Families) |

## Ventas

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/sales/orders` | Lista pedidos |
| GET | `/api/sales/orders/{id}` | Obtener pedido |
| POST | `/api/sales/orders` | Crear pedido con líneas |
| POST | `/api/sales/orders/{id}/confirm` | Confirmar pedido |
| POST | `/api/sales/orders/{id}/cancel` | Cancelar pedido |
| GET | `/api/sales/delivery-notes` | Lista albaranes venta |
| GET | `/api/sales/delivery-notes/{id}` | Obtener albarán |
| POST | `/api/sales/delivery-notes` | Crear albarán |
| POST | `/api/sales/delivery-notes/{id}/post` | Emitir albarán |
| POST | `/api/sales/delivery-notes/from-order/{orderId}` | Generar desde pedido |
| POST | `/api/sales/invoices/from-delivery-note/{id}` | Generar factura desde albarán |
| GET | `/api/sales/invoices` | Lista facturas venta |
| GET | `/api/sales/invoices/{id}` | Obtener factura |
| POST | `/api/sales/invoices` | Crear factura |
| POST | `/api/sales/invoices/{id}/post` | Contabilizar factura |
| POST | `/api/sales/invoices/{id}/cancel` | Cancelar factura |
| GET | `/api/sales/credit-notes` | Lista rectificativas |
| POST | `/api/sales/credit-notes` | Crear rectificativa |
| POST | `/api/sales/credit-notes/{id}/post` | Contabilizar rectificativa |
| GET | `/api/customers/payments` | Lista cobros |
| POST | `/api/customers/payments` | Registrar cobro |

## Compras (simétrico a Ventas)

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/purchasing/orders` | Lista pedidos compra |
| POST | `/api/purchasing/orders` | Crear pedido compra |
| ... | ... | Estructura idéntica a ventas |
| GET | `/api/supplier-payments` | Lista pagos proveedor |
| POST | `/api/supplier-payments` | Registrar pago |

## Inventario

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/warehouses` | Lista almacenes |
| GET | `/api/warehouses/{id}` | Obtener almacén |
| POST | `/api/warehouses` | Crear almacén |
| POST | `/api/warehouses/{id}/locations` | Añadir ubicación |
| GET | `/api/stock/movements` | Lista movimientos |
| POST | `/api/stock/movements` | Crear movimiento |
| GET | `/api/stock/balance` | Saldo stock (con filtros) |

## Contabilidad

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/accounting/accounts` | Lista cuentas |
| GET | `/api/accounting/accounts/{id}` | Obtener cuenta |
| POST | `/api/accounting/accounts` | Crear cuenta |
| GET | `/api/accounting/fiscal-years` | Lista ejercicios |
| POST | `/api/accounting/fiscal-years` | Crear ejercicio |
| GET | `/api/accounting/journals` | Lista diarios |
| POST | `/api/accounting/journals` | Crear diario |
| GET | `/api/accounting/entries` | Lista asientos |
| GET | `/api/accounting/entries/{id}` | Obtener asiento |
| POST | `/api/accounting/entries` | Crear asiento |
| POST | `/api/accounting/entries/{id}/post` | Contabilizar asiento |

## IA

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/ai/customers/{id}/chat` | Chat con contexto CRM del cliente |
| GET | `/api/ai/customers/{id}/summary` | Resumen CRM del cliente |

## Licenciamiento

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/licenses/current` | Licencia activa |
| POST | `/api/licenses/activate` | Activar licencia |
| GET | `/api/subscription-plans` | Lista planes |
