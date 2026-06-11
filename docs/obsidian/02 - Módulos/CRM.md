---
type: module
module: crm
layer: cross
status: implemented
source:
  - src/Debales.Domain/CRM/
  - src/Debales.Application/CRM/
  - src/Debales.Infrastructure/Persistence/Repositories/CRM/
  - src/Debales.Api/Controllers/CustomersController.cs
  - src/Debales.Api/Controllers/CustomerPaymentsController.cs
  - src/Debales.Web/Components/Pages/CRM/
related:
  - Customer
  - Contact
  - Activity
  - Note
  - Opportunity
  - DbContext
---

# Módulo CRM

## Qué problema resuelve

Gestión de relaciones con clientes: datos, contactos, actividades comerciales, notas y oportunidades de venta.

## Estado

Implementado — migración `AddCrmModule` (2026-05-27) + `AddCustomerEmail` (2026-05-28).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[Customer]] | Cliente con nombre, NIF, sector, email, dirección y código contable |
| [[Contact]] | Contacto asociado a un cliente |
| [[Activity]] | Actividad comercial (llamada, reunión, email, visita) |
| [[Note]] | Nota libre asociada a un cliente |
| [[Opportunity]] | Oportunidad de venta con estado y valor estimado |
| [[Address]] | Value Object embebido en Customer |

## Handlers

| Handler | Tipo |
|---------|------|
| `CreateCustomerHandler` | Command |
| `UpdateCustomerHandler` | Command |
| `GetCustomersHandler` | Query (paginado) |
| `GetCustomerByIdHandler` | Query |
| `AddContactHandler` | Command |
| `GetContactsByCustomerHandler` | Query |
| `LogActivityHandler` | Command |
| `CompleteActivityHandler` | Command |
| `GetActivitiesByCustomerHandler` | Query |
| `AddNoteHandler` | Command |
| `GetNotesByCustomerHandler` | Query |
| `CreateOpportunityHandler` | Command |
| `UpdateOpportunityStatusHandler` | Command |
| `GetOpportunitiesByCustomerHandler` | Query |

## Controllers

| Controller | Ruta base |
|------------|-----------|
| `CustomersController` | `api/customers` |
| `CustomerPaymentsController` | `api/customers/payments` |

## Páginas Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Customers.razor` | `/crm/customers` | Implementada — lista con búsqueda y paginación |
| `CustomerDetail.razor` | `/crm/customers/{id}` | Implementada — ficha con tabs (contactos, actividades, notas, oportunidades) |

## Repositorios

- `ICustomerRepository` → `CustomerRepository`
- `IContactRepository` → `ContactRepository`
- `IActivityRepository` → `ActivityRepository`
- `INoteRepository` → `NoteRepository`
- `IOpportunityRepository` → `OpportunityRepository`

## Lo que está completo

- CRUD de clientes con búsqueda y paginación
- Gestión de contactos, actividades, notas y oportunidades
- Dirección embebida como value object
- Ficha de cliente con tabs en UI (info, contactos, actividades, notas, oportunidades, historial, códigos, pedidos, estado de cuenta, asistente IA)
- Código contable de cliente (`AccountCode`) para integración contable
- Tab "Pedidos" — historial lazy de pedidos de venta del cliente, con badge estado y navegación a ficha

## Lo que falta

- Importación masiva de clientes
- Historial de cambios visible desde UI
