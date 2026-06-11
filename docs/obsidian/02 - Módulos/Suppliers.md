---
type: module
module: crm
layer: cross
status: implemented
source:
  - src/Debales.Domain/Suppliers/
  - src/Debales.Application/Suppliers/
  - src/Debales.Infrastructure/Persistence/Repositories/Suppliers/
  - src/Debales.Api/Controllers/SuppliersController.cs
  - src/Debales.Api/Controllers/SupplierPaymentsController.cs
  - src/Debales.Web/Components/Pages/Proveedores/
related:
  - Supplier
  - SupplierAddress
  - DbContext
---

# Módulo Suppliers (Proveedores)

## Qué problema resuelve

Gestión del catálogo de proveedores: datos maestros, dirección, contacto y soft-delete.

## Estado

Implementado — migración `AddSuppliersModule` (2026-05-28).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Supplier]] | Proveedor con nombre, NIF, contacto, email, web, notas y código contable |
| [[SupplierAddress]] | Value Object embebido en Supplier (calle, ciudad, CP, país) |

## Handlers

| Handler | Tipo |
|---------|------|
| `CreateSupplierHandler` | Command |
| `UpdateSupplierHandler` | Command |
| `GetSuppliersHandler` | Query (paginado) |
| `GetSupplierByIdHandler` | Query |

## Controllers

| Controller | Ruta base |
|------------|-----------|
| `SuppliersController` | `api/suppliers` |
| `SupplierPaymentsController` | `api/supplier-payments` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Suppliers.razor` | `/proveedores` | Implementada — lista con búsqueda y paginación |
| `SupplierDetail.razor` | `/proveedores/{id}` | Implementada — ficha de proveedor |

## Repositorios

- `ISupplierRepository` → `SupplierRepository`

## Lo que está completo

- CRUD de proveedores con búsqueda y paginación
- Soft-delete (`IsActive = false`)
- Dirección embebida como value object
- Código contable de proveedor (`AccountCode`) para integración contable
- Ficha de proveedor con tabs (info, notas, códigos, pedidos, estado de cuenta)
- Tab "Pedidos" — historial lazy de pedidos de compra del proveedor, con badge estado y navegación a ficha

## Lo que falta

- Contactos múltiples de proveedor (solo hay `ContactName` como string)
