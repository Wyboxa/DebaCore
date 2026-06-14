---
type: ui
module: cross
layer: web
status: implemented
source:
  - src/Debales.Web/Components/Pages/
  - src/Debales.Web/Components/Layout/NavMenu.razor
related:
  - Índice UI Blazor
---

# Rutas Blazor

Mapa de todas las rutas `@page` encontradas en componentes Razor.

| Ruta | Archivo | Estado |
|------|---------|--------|
| `/` | Home.razor | Implementada |
| `/login` | Login.razor | Implementada |
| `/crm/customers` | CRM/Customers.razor | Implementada |
| `/crm/customers/{id:guid}` | CRM/CustomerDetail.razor | Implementada |
| `/proveedores` | Proveedores/Suppliers.razor | Implementada |
| `/proveedores/{id:guid}` | Proveedores/SupplierDetail.razor | Implementada |
| `/catalogo` | Catalogo/Items.razor | Implementada |
| `/catalogo/{id:guid}` | Catalogo/ItemDetail.razor | Implementada |
| `/ventas` | Ventas/Ventas.razor | Implementada — hub con datos reales |
| `/ventas/pedidos` | Ventas/Pedidos.razor | Implementada |
| `/ventas/pedidos/{id:guid}` | Ventas/PedidoDetalle.razor | Implementada |
| `/ventas/albaranes` | Ventas/AlbaranesVenta.razor | Implementada |
| `/ventas/albaranes/{id:guid}` | Ventas/AlbaranVentaDetalle.razor | Implementada |
| `/ventas/automatizacion` | Ventas/Automatizacion.razor | Implementada |
| `/facturacion` | Facturacion/Facturacion.razor | Implementada — hub con datos reales, vencimientos pendientes |
| `/facturacion/ventas` | Facturacion/FacturasVenta.razor | Implementada |
| `/facturacion/ventas/{id:guid}` | Facturacion/FacturaVentaDetalle.razor | Implementada |
| `/facturacion/rectificativas-venta` | Facturacion/RectificativasVenta.razor | Implementada |
| `/facturacion/rectificativas-venta/{id:guid}` | Facturacion/RectificativaVentaDetalle.razor | Implementada |
| `/facturacion/compras` | Facturacion/FacturasCompra.razor | Implementada |
| `/facturacion/compras/{id:guid}` | Facturacion/FacturaCompraDetalle.razor | Implementada |
| `/facturacion/rectificativas-compra` | Facturacion/RectificativasCompra.razor | Implementada |
| `/facturacion/rectificativas-compra/{id:guid}` | Facturacion/RectificativaCompraDetalle.razor | Implementada |
| `/compras` | Compras/Compras.razor | Implementada — hub con datos reales, pagos pendientes |
| `/compras/pedidos` | Compras/Pedidos.razor | Implementada |
| `/compras/pedidos/{id:guid}` | Compras/PedidoDetalle.razor | Implementada |
| `/compras/albaranes` | Compras/AlbaranesCompra.razor | Implementada |
| `/compras/albaranes/{id:guid}` | Compras/AlbaranCompraDetalle.razor | Implementada |
| `/inventario` | Inventario/Inventario.razor | Implementada — hub con datos reales, tabla de stock |
| `/inventario/almacenes` | Inventario/Almacenes.razor | Implementada |
| `/inventario/movimientos` | Inventario/Movimientos.razor | Implementada |
| `/inventario/saldos` | Inventario/SaldosStock.razor | Implementada |
| `/contabilidad/plan` | Contabilidad/PlanContable.razor | Implementada |
| `/contabilidad/ejercicios` | Contabilidad/EjerciciosFiscales.razor | Implementada |
| `/contabilidad/asientos` | Contabilidad/Asientos.razor | Implementada |
| `/ia` | IA/IA.razor | Implementada |
| `/licencia` | Licencia/Licencia.razor | Implementada |
| `/analitica` | Analitica/Analitica.razor | Implementada — KPIs reales, top 5 clientes, últimas facturas |
| `/configuracion` | Configuracion/Configuracion.razor | Implementada — datos reales del sistema |
| `/configuracion/usuarios` | Configuracion/Usuarios/Usuarios.razor | Implementada — lista, búsqueda, crear usuario |
| `/configuracion/usuarios/{id:guid}` | Configuracion/Usuarios/UsuarioDetalle.razor | Implementada — detalle, activar/desactivar, cambiar contraseña, asignar rol |
| `/configuracion/series` | Configuracion/Series.razor | Implementada — series documentales configurables |
| `/configuracion/formas-pago` | Configuracion/FormasPago.razor | Implementada — CRUD formas de pago |
| `/configuracion/condiciones-pago` | Configuracion/CondicionesPago.razor | Implementada — CRUD condiciones de pago |
| `/configuracion/auditoria` | Configuracion/Auditoria.razor | Implementada — log de auditoría con filtros |
| `/configuracion/tipos-documento` | Configuracion/TiposDocumento.razor | Implementada — CRUD tipos de documento |
| `/configuracion/ai-reglas` | Configuracion/AIReglas.razor | Implementada — CRUD reglas IA |
| `/configuracion/ai-conocimiento` | Configuracion/AIConocimiento.razor | Implementada — CRUD base de conocimiento IA |
| `/documentos` | Documentos/Documentos.razor | Implementada — lista paginada de documentos con búsqueda |
| `/documentos/{id:guid}` | Documentos/DocumentoDetalle.razor | Implementada — ficha de documento |
| `/ventas/presupuestos` | Ventas/Presupuestos.razor | Implementada — lista presupuestos de venta |
| `/ventas/presupuestos/{id:guid}` | Ventas/PresupuestoDetalle.razor | Implementada — ciclo completo hasta pedido |
| `/contabilidad/informes` | Contabilidad/Informes.razor | Implementada — balance comprobación, libro diario, balance situación |
| `/contabilidad/cuentas-bancarias` | Contabilidad/CuentasBancarias.razor | Implementada — CRUD cuentas bancarias |
| `/contabilidad/cajas` | Contabilidad/Cajas.razor | Implementada — CRUD cajas |
| `/contabilidad/remesas` | Contabilidad/Remesas.razor | Implementada — lista remesas bancarias |
| `/contabilidad/remesas/{id:guid}` | Contabilidad/RemesaDetalle.razor | Implementada — detalle + líneas + ciclo de vida |
| `/contabilidad/vencimientos` | Contabilidad/Vencimientos.razor | Implementada — aging report cobros/pagos |
| `/contabilidad/tesoreria` | Contabilidad/Tesoreria.razor | Implementada — posición tesorería |
| `/contabilidad/estado-cuenta-clientes` | Contabilidad/EstadoCuentaClientes.razor | Implementada — estado de cuenta por cliente |
| `/contabilidad/estado-cuenta-proveedores` | Contabilidad/EstadoCuentaProveedores.razor | Implementada — estado de cuenta por proveedor |
| `/inventario/conteos` | Inventario/ConteosInventario.razor | Implementada — lista recuentos físicos |
| `/inventario/conteos/{id:guid}` | Inventario/ConteoInventarioDetalle.razor | Implementada — sesión de recuento físico |
| `/catalogo/tarifas` | Catalogo/Tarifas.razor | Implementada — CRUD tarifas de precio |
| `/catalogo/tarifas/{id:guid}` | Catalogo/TarifaDetalle.razor | Implementada — detalle tarifa con precios por artículo |
| `/licencia` | Licencia/Licencia.razor | Implementada — gestión de licencia y plan |
| `/ai/propuestas` | AI/Propuestas.razor | Implementada — propuestas IA con flujo de aprobación |
| `/ai/propuestas/{id:guid}` | AI/PropuestaDetalle.razor | Implementada — detalle con payload y historial de revisiones |

## Componentes compartidos (Shared/)

| Componente | Descripción |
|------------|-------------|
| `ModuleRequired.razor` | Guard de licencia: si hay licencia pero el módulo no está activo, bloquea con mensaje. Si no hay licencia (nueva instalación), deja pasar. |
| `BlazorCookieAuthStateProvider.cs` | Lee el HttpContext en el arranque del circuito para proporcionar el estado de autenticación |

## Endpoints de descarga (minimal API en Web/Program.cs)

| Ruta | Descripción |
|------|-------------|
| `GET /descargar/factura-venta/{id}` | Genera y descarga PDF de factura de venta (QuestPDF) |
| `GET /descargar/factura-compra/{id}` | Genera y descarga PDF de factura de compra (QuestPDF) |

Ambos endpoints están en `Debales.Web/Program.cs` — misma origin que la UI, con `RequireAuthorization()`.

## Navegación del sidebar (NavMenu)

El sidebar tiene las siguientes secciones:
- **CRM**: Inicio, Clientes, Proveedores
- **Operaciones**: Catálogo, Ventas, Albaranes venta, Fact. Venta, Rectif. Venta, Fact. Compra, Rectif. Compra, Compras, Albaranes compra, Automatización, Inventario
- **Contabilidad**: Ejercicios, Plan contable, Asientos
- **Análisis**: Analítica, Asistente IA (badge "NUEVO")
- **Footer**: Licencia, Configuración, Info de plan
