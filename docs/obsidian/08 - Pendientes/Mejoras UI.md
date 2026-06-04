---
type: audit
module: cross
layer: web
status: pending
related:
  - Índice UI Blazor
  - Pendientes priorizados
---

# Mejoras UI

## Páginas placeholder (sin funcionalidad real)

| Página | Ruta | Situación |
|--------|------|-----------|
| Ventas.razor | `/ventas` | Placeholder de sección |
| Compras.razor | `/compras` | Placeholder de sección |
| Inventario.razor | `/inventario` | Placeholder de sección |
| Facturacion.razor | `/facturacion` | Placeholder de sección |
| Configuracion.razor | `/configuracion` | Placeholder sin funcionalidad |

## Funcionalidades ausentes en UI

### Gestión de usuarios
No hay páginas para administrar usuarios, roles ni permisos desde la UI. Solo existe el endpoint API.

### Cobros y pagos en UI
Los vencimientos (`Receivables`, `Payables`) y cobros/pagos (`CustomerPayment`, `SupplierPayment`) tienen endpoints API pero no se confirmó si tienen páginas de lista en UI (los handlers están registrados, pero no se encontraron rutas `/cobros` o `/pagos`).

### Analítica
La página `/analitica` existe pero el nivel de implementación no se confirmó durante la auditoría.

### Cierre de ejercicio/período desde UI
Los handlers `CloseFiscalPeriodHandler` y `CloseFiscalYearHandler` existen, pero no hay endpoint API ni página UI que los invoque (no confirmado).

## Mejoras de UX sugeridas

- Confirmación antes de acciones destructivas (cancelar factura, cancelar pedido)
- Filtros de fecha en listas de documentos
- Export a CSV/Excel en listas
- Paginación persistente al navegar atrás
- Búsqueda global
