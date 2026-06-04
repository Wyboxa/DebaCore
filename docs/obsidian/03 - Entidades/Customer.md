---
type: entity
module: crm
layer: domain
status: implemented
source:
  - src/Debales.Domain/CRM/Customers/Customer.cs
related:
  - CRM
  - Contact
  - Activity
  - Note
  - Opportunity
  - Address
  - SalesOrder
  - SalesInvoice
---

# Customer

## Tabla EF / DbSet

`Customers` — `DbSet<Customer>`

## Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Id` | `Guid` | PK (heredado de Entity) |
| `Name` | `string` | Nombre del cliente (obligatorio) |
| `Sector` | `string?` | Sector de actividad |
| `TaxId` | `string?` | NIF/CIF |
| `Phone` | `string?` | Teléfono |
| `Email` | `string?` | Email (lowercase) |
| `Website` | `string?` | Sitio web |
| `IsActive` | `bool` | Activo/inactivo (soft-delete lógico) |
| `AccountCode` | `string?` | Código contable para integración con Contabilidad |
| `Address` | `Address?` | Value object embebido |
| `CreatedAt` | `DateTime` | Herencia AuditableEntity |
| `CreatedBy` | `string` | Herencia AuditableEntity |
| `UpdatedAt` | `DateTime?` | Herencia AuditableEntity |
| `UpdatedBy` | `string?` | Herencia AuditableEntity |

## Colecciones de navegación

| Colección | Tipo | Relación |
|-----------|------|----------|
| `Contacts` | `IReadOnlyList<Contact>` | 1:N (explícita) |
| `Activities` | `IReadOnlyList<Activity>` | 1:N (explícita) |
| `Notes` | `IReadOnlyList<Note>` | 1:N (explícita) |
| `Opportunities` | `IReadOnlyList<Opportunity>` | 1:N (explícita) |

## Métodos de dominio

| Método | Descripción |
|--------|-------------|
| `Create(name, sector, taxId, phone, email, createdBy)` | Factory estático con validación |
| `Update(name, sector, taxId, phone, email, website, updatedBy)` | Actualización completa |
| `SetAddress(address, updatedBy)` | Asigna dirección embebida |
| `Deactivate(updatedBy)` | Soft-delete (IsActive = false) |
| `SetAccountCode(accountCode, updatedBy)` | Vincula con código contable |

## DTOs relacionados

- `CustomerSummaryDto` — lista paginada
- `CustomerDetailDto` — ficha completa
- Handlers: `CreateCustomerHandler`, `UpdateCustomerHandler`, `GetCustomersHandler`, `GetCustomerByIdHandler`

## Tests

`tests/Debales.Domain.Tests/CRM/Customers/CustomerTests.cs` — tests de creación y reglas de dominio.
