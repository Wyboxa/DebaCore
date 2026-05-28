# Módulo CRM — Debales

## Estado

**Implementado — Fase 3 completada**

## Dependencias

- Core 1.0.0

## Propósito

Primer módulo funcional real. Gestión de relaciones con clientes.

## Implementado

### Dominio (`Debales.Domain/CRM/`)

- `Customer` con value object `Address`
- `Contact`
- `Activity` + enum `ActivityType` (Call, Email, Meeting, Task, Other)
- `Note`
- `Opportunity` + enum `OpportunityStatus` (New, InProgress, Won, Lost)

### Application (`Debales.Application/CRM/`)

- Customers: `CreateCustomerCommand`, `UpdateCustomerCommand`, `GetCustomersQuery` (paginada), `GetCustomerByIdQuery`
- Contacts: `AddContactCommand`, `GetContactsByCustomerQuery`
- Activities: `LogActivityCommand`, `GetActivitiesByCustomerQuery`
- Notes: `AddNoteCommand`, `GetNotesByCustomerQuery`
- Opportunities: `CreateOpportunityCommand`, `UpdateOpportunityStatusCommand`, `GetOpportunitiesByCustomerQuery`
- DTOs: `CustomerSummaryDto`, `CustomerDetailDto`, `ContactDto`, `ActivityDto`, `NoteDto`, `OpportunityDto`
- `PagedResult<T>` para paginación

### Infrastructure (`Debales.Infrastructure/Persistence/`)

- Repositorios: `CustomerRepository`, `ContactRepository`, `ActivityRepository`, `NoteRepository`, `OpportunityRepository`
- Configuraciones EF Core para todas las entidades CRM

### API REST — 11 endpoints operativos

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/customers` | Lista paginada con búsqueda |
| GET | `/api/customers/{id}` | Ficha de cliente |
| POST | `/api/customers` | Crear cliente |
| PUT | `/api/customers/{id}` | Actualizar cliente |
| GET | `/api/customers/{id}/contacts` | Contactos del cliente |
| POST | `/api/customers/{id}/contacts` | Añadir contacto |
| GET | `/api/customers/{id}/activities` | Actividades |
| POST | `/api/customers/{id}/activities` | Registrar actividad |
| POST | `/api/customers/{id}/notes` | Añadir nota |
| GET | `/api/customers/{id}/opportunities` | Oportunidades |
| POST | `/api/customers/{id}/opportunities` | Crear oportunidad |
| PATCH | `/api/customers/{id}/opportunities/{oppId}/status` | Cambiar estado |

### UI Blazor Server — 2 páginas operativas

- `/crm/customers` — Lista con búsqueda, paginación y modal de creación
- `/crm/customers/{id}` — Ficha con 5 pestañas: Información, Contactos, Actividades, Notas, Oportunidades

## Tablas en BD

```
Customers
Contacts
Activities
Notes
Opportunities
```

## Permisos del módulo

```
crm.customers.read
crm.customers.write
crm.contacts.read
crm.contacts.write
crm.activities.read
crm.activities.write
crm.notes.read
crm.notes.write
crm.opportunities.read
crm.opportunities.write
```

## Bugs conocidos (pendientes P0)

- Sin autenticación: todos los endpoints son accesibles sin credenciales.
- `GET /api/customers/{id}/notes` no existe (solo POST).
- `UpdateOpportunityStatus` no valida que la oportunidad pertenece al cliente de la ruta.

Ver detalle en `estado_actual.md §3` y `roadmap.md — Prioridad 0`.
