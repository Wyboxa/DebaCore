---
type: audit
module: cross
layer: database
status: pending
related:
  - Seeds
  - Pendientes priorizados
---

# Mejoras de datos demo

## Estado actual del DemoDataSeeder

El `DemoDataSeeder` crea datos de demostración útiles para ventas pero incompletos para el resto de módulos:

**Tiene:**
- 3 clientes demo (Herrera, Norte, Pérez)
- 5 artículos (3 productos + 2 servicios)
- 1 almacén (ALM-01)
- Stock inicial de 3 artículos
- 3 escenarios de pedido de venta (uno completo hasta factura)

**No tiene:**
- Proveedores demo
- Pedidos de compra demo
- Facturas de compra demo
- Ejercicio fiscal demo con períodos
- Asientos contables demo
- Licencia demo preactivada

## Impacto

Al arrancar la aplicación por primera vez:
- La UI de IA no tiene contexto financiero real para analizar
- La UI de contabilidad arranca vacía (sin ejercicio, sin asientos)
- La UI de licencia muestra "Sin licencia activa"
- La UI de compras está vacía

## Mejoras recomendadas

1. Añadir 2-3 proveedores demo (paralelo a los clientes)
2. Añadir escenarios de compra (pedido → albarán → factura)
3. Crear un ejercicio fiscal 2026 con períodos mensuales
4. Crear algunos asientos demo (al menos los de las facturas demo)
5. Pre-activar una licencia Trial demo
6. Añadir cobros demo para las facturas creadas
