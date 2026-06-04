---
type: entity
module: ventas
layer: domain
status: implemented
source:
  - src/Debales.Domain/Sales/SalesOrder.cs
  - src/Debales.Domain/Sales/SalesOrderLine.cs
  - src/Debales.Domain/Sales/SalesOrderStatus.cs
related:
  - Ventas
  - Customer
  - SalesDeliveryNote
  - SalesInvoice
  - Item
---

# SalesOrder (Pedido de Venta)

## Tabla EF / DbSet

`SalesOrders` — `DbSet<SalesOrder>`
`SalesOrderLines` — `DbSet<SalesOrderLine>`

## Propiedades de SalesOrder

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Number` | `string` | Número de pedido (único, uppercase) |
| `CustomerId` | `Guid` | FK a Customer (relación explícita) |
| `Date` | `DateOnly` | Fecha del pedido |
| `RequestedDeliveryDate` | `DateOnly?` | Fecha de entrega solicitada |
| `Status` | `SalesOrderStatus` | Estado actual |
| `Notes` | `string?` | Observaciones |
| `Customer` | `Customer?` | Navigation property (EF only) |

## Propiedades calculadas

| Propiedad | Descripción |
|-----------|-------------|
| `Subtotal` | Suma de `LineSubtotal` de todas las líneas |
| `TaxAmount` | Suma de `LineTaxAmount` |
| `Total` | Suma de `LineTotal` |

## Estados (SalesOrderStatus)

```
Draft → Confirmed → PartiallyDelivered → Delivered
                  ↘ Cancelled
```

## SalesOrderLine — propiedades clave

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `ItemId` | `Guid` | FK a Item |
| `ItemCode` | `string` | Código denormalizado |
| `ItemName` | `string` | Nombre denormalizado |
| `Quantity` | `decimal` | Cantidad pedida |
| `DeliveredQuantity` | `decimal` | Acumulado albaranes |
| `PendingQuantity` | `decimal` | Calculado: Quantity - DeliveredQuantity |
| `UnitPrice` | `decimal` | Precio unitario |
| `TaxRate` | `decimal` | Porcentaje IVA |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(...)` | Factory con validación de número y cliente |
| `AddLine(itemId, ..., taxRate)` | Añade línea (solo en Draft) |
| `Confirm(updatedBy)` | Draft → Confirmed (requiere líneas) |
| `Cancel(updatedBy)` | Cancela si no está Delivered ni Cancelled |
| `UpdateDeliveryStatus(updatedBy)` | Recalcula estado según cantidades entregadas |

## Relaciones

| Relación | Tipo | Confirmada |
|----------|------|-----------|
| Customer (FK CustomerId) | N:1 | Explícita |
| SalesOrderLine | 1:N | Explícita |
| SalesDeliveryNote (referencia SalesOrderId) | 1:N | Explícita |

## Handlers que usan esta entidad

- `CreateSalesOrderHandler`, `ConfirmSalesOrderHandler`, `CancelSalesOrderHandler`
- `GenerateDeliveryNoteFromOrderHandler` — lee líneas del pedido para crear albarán
