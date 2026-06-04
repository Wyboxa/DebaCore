---
type: api
module: cross
layer: web
status: implemented
source:
  - src/Debales.Application/Documents/IInvoicePdfGenerator.cs
  - src/Debales.Infrastructure/Documents/InvoicePdfGenerator.cs
  - src/Debales.Web/Program.cs
related:
  - FacturaVentaDetalle
  - FacturaCompraDetalle
  - Índice API
---

# PDF Export — Facturas

## Implementado en 2026-06-05

Generación y descarga de facturas en PDF usando QuestPDF Community.

## Endpoints (Debales.Web/Program.cs)

Los endpoints viven en `Debales.Web`, **no** en `Debales.Api`. Esto es intencional: los archivos descargables deben servirse desde el mismo origen que la UI para que los enlaces `<a href="...">` funcionen sin CORS.

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/descargar/factura-venta/{id:guid}` | Descarga PDF de factura de venta |
| GET | `/descargar/factura-compra/{id:guid}` | Descarga PDF de factura de compra |

Ambos requieren autenticación (`RequireAuthorization()`).

## Flujo

```
GET /descargar/factura-venta/{id}
  → GetSalesInvoiceByIdHandler.Handle()       ← carga DTO completo con líneas
  → IInvoicePdfGenerator.GenerateSalesInvoice()
  → Results.File(bytes, "application/pdf", "Factura-XXX.pdf")
```

## Interfaz

```csharp
// Debales.Application/Documents/IInvoicePdfGenerator.cs
public interface IInvoicePdfGenerator {
    byte[] GenerateSalesInvoice(SalesInvoiceDetailDto invoice);
    byte[] GeneratePurchaseInvoice(PurchaseInvoiceDetailDto invoice);
}
```

## Implementación

`InvoicePdfGenerator` en `Debales.Infrastructure/Documents/` — registrado como **Singleton** (QuestPDF es thread-safe).

Características del PDF:
- Formato A4 vertical
- Cabecera teal `#6B9CA9` con nombre "Debales" y número de factura
- Tabla de líneas con columnas: #, Código, Descripción, Cantidad, Precio unit., IVA%, Subtotal, Total
- Filas alternadas (fondo gris claro)
- Bloque de totales oscuro al final: Subtotal, Total IVA, **TOTAL**
- Pie de página con número de página

## UI

En `FacturaVentaDetalle.razor` y `FacturaCompraDetalle.razor` hay un botón:

```html
<a href="/descargar/factura-venta/@_invoice.Id" target="_blank" download>
    Descargar PDF
</a>
```

El atributo `download` dispara la descarga directa. `target="_blank"` abre en nueva pestaña como fallback.

## Dependencia NuGet

`QuestPDF` (versión 2026.x) — Licencia Community (gratuita para proyectos con ingresos < $1M/año).

La licencia se activa en el constructor estático:
```csharp
QuestPDF.Settings.License = LicenseType.Community;
```
