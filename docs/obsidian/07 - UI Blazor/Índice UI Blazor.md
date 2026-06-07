---
type: index
module: cross
layer: web
status: implemented
source:
  - src/Debales.Web/Components/Pages/
related:
  - Rutas Blazor
  - 01 - Arquitectura
---

# Índice UI Blazor

**Framework**: Blazor Server con `@rendermode InteractiveServer`  
**Autenticación**: `@attribute [Authorize]` en todas las páginas funcionales  
**Paleta**: Teal `#6B9CA9` — sidebar oscuro  

## Páginas por sección

### CRM

| Página | Ruta | Estado |
|--------|------|--------|
| Customers.razor | `/crm/customers` | Implementada — lista + búsqueda + paginación |
| CustomerDetail.razor | `/crm/customers/{id}` | Implementada — ficha con tabs |

### Proveedores

| Página | Ruta | Estado |
|--------|------|--------|
| Suppliers.razor | `/proveedores` | Implementada |
| SupplierDetail.razor | `/proveedores/{id}` | Implementada |

### Catálogo

| Página | Ruta | Estado |
|--------|------|--------|
| Items.razor | `/catalogo` | Implementada |
| ItemDetail.razor | `/catalogo/{id}` | Implementada |

### Ventas

| Página | Ruta | Estado |
|--------|------|--------|
| Ventas.razor | `/ventas` | Placeholder de sección |
| Pedidos.razor | `/ventas/pedidos` | Implementada — lista + modal creación con líneas |
| PedidoDetalle.razor | `/ventas/pedidos/{id}` | Implementada |
| AlbaranesVenta.razor | `/ventas/albaranes` | Implementada |
| AlbaranVentaDetalle.razor | `/ventas/albaranes/{id}` | Implementada |
| Automatizacion.razor | `/ventas/automatizacion` | Implementada — batch pedido→albarán→factura |

### Facturación

| Página | Ruta | Estado |
|--------|------|--------|
| Facturacion.razor | `/facturacion` | Placeholder |
| FacturasVenta.razor | `/facturacion/ventas` | Implementada |
| FacturaVentaDetalle.razor | `/facturacion/ventas/{id}` | Implementada |
| RectificativasVenta.razor | `/facturacion/rectificativas-venta` | Implementada |
| RectificativaVentaDetalle.razor | `/facturacion/rectificativas-venta/{id}` | Implementada |
| FacturasCompra.razor | `/facturacion/compras` | Implementada |
| FacturaCompraDetalle.razor | `/facturacion/compras/{id}` | Implementada |
| RectificativasCompra.razor | `/facturacion/rectificativas-compra` | Implementada |
| RectificativaCompraDetalle.razor | `/facturacion/rectificativas-compra/{id}` | Implementada |

### Compras

| Página | Ruta | Estado |
|--------|------|--------|
| Compras.razor | `/compras` | Placeholder |
| Pedidos.razor | `/compras/pedidos` | Implementada |
| PedidoDetalle.razor | `/compras/pedidos/{id}` | Implementada |
| AlbaranesCompra.razor | `/compras/albaranes` | Implementada |
| AlbaranCompraDetalle.razor | `/compras/albaranes/{id}` | Implementada |

### Inventario

| Página | Ruta | Estado |
|--------|------|--------|
| Inventario.razor | `/inventario` | Placeholder |
| Almacenes.razor | `/inventario/almacenes` | Implementada |
| Movimientos.razor | `/inventario/movimientos` | Implementada |
| SaldosStock.razor | `/inventario/saldos` | Implementada |

### Contabilidad

| Página | Ruta | Estado |
|--------|------|--------|
| PlanContable.razor | `/contabilidad/plan` | Implementada |
| EjerciciosFiscales.razor | `/contabilidad/ejercicios` | Implementada |
| Asientos.razor | `/contabilidad/asientos` | Implementada |

### IA

| Página | Ruta | Estado |
|--------|------|--------|
| IA.razor | `/ia` | Implementada — 4 tabs funcionales |

### Licencia

| Página | Ruta | Estado |
|--------|------|--------|
| Licencia.razor | `/licencia` | Implementada — estado + activación |

### Sistema

| Página | Ruta | Estado |
|--------|------|--------|
| Home.razor | `/` | Implementada — 6 KPIs con importes € + alertas cobros/pagos vencidos + actividad reciente |
| Login.razor | `/login` | Implementada |
| Analitica.razor | `/analitica` | Placeholder |
| Configuracion.razor | `/configuracion` | Implementada — usuarios, licencia, tutorial, auditoría, series documentales |
| Usuarios.razor | `/configuracion/usuarios` | Implementada |
| Auditoria.razor | `/configuracion/auditoria` | Implementada — filtros entidad/fecha, tabla con JSON expandible |
| SeriesDocumentales.razor | `/configuracion/series` | Implementada — tabla series, crear/editar con vista previa en vivo |

**Total páginas Razor: 46**

## Componentes compartidos

| Componente | Descripción |
|------------|-------------|
| `NavMenu.razor` | Sidebar de navegación con secciones y badges |
| `MainLayout.razor` | Layout principal |
| `ModulePlaceholder.razor` | Componente para páginas en construcción |
| `ToastContainer.razor` | Notificaciones toast |
| `TutorialOverlay.razor` | Card flotante de tutorial guiado (8 pasos, persistencia localStorage) |
| `RedirectToLogin.razor` | Redirect a login si no autenticado |

## Servicios Web

| Servicio | Descripción |
|----------|-------------|
| `ToastService` | Notificaciones toast |
| `TutorialService` | Estado del tutorial (localStorage), 8 pasos, activable desde Configuración |
