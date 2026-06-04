---
type: audit
module: cross
layer: cross
status: not_confirmed
related:
  - Pendientes priorizados
  - 01 - Arquitectura
---

# Deuda técnica

## Arquitectura

### Web accede a Infrastructure directamente
`Debales.Web` referencia `Debales.Infrastructure` además de `Debales.Application`. En Blazor Server esto es pragmático (mismo proceso), pero viola la separación de capas declarada en CLAUDE.md. Los handlers de Application son el punto de acceso correcto.

**Riesgo**: Futuro acoplamiento de código UI a infraestructura.

### Sin mediator — handlers registrados como servicios Scoped
El patrón CQRS se implementa sin MediatR: cada handler se registra directamente en DI y se inyecta en controllers y páginas Blazor. Esto genera un `DependencyInjection.cs` en Application con ~80 registros y controllers con múltiples parámetros en constructor.

**Riesgo**: Escalabilidad del mantenimiento. Si se añaden más handlers, el archivo de DI crece indefinidamente.

### IA ERP-6 sin endpoints API propios
`ChatWithERPHandler`, `GetERPAnomaliesHandler`, `GetCustomerERPSummaryHandler` y `GetSupplierERPSummaryHandler` solo se usan desde la UI Blazor. No hay endpoints API para estas funcionalidades.

**Riesgo**: No se puede usar la IA ERP-6 desde otros clientes o integraciones externas.

## Tests

### Cobertura limitada
Los tests actuales cubren:
- Dominio: Customer, Opportunity, Role, User, Entity
- Application: CreateCustomer, CreateUser handlers, contratos de repositorio
- Integration: Smoke tests

No hay tests para:
- Módulos ERP (Sales, Purchasing, Inventory, Accounting)
- Handlers de Licensing
- Handlers de IA
- Flujos end-to-end

### No hay tests de migración
Las 10 migraciones no tienen tests automatizados de verificación.

## Seguridad

### Licencia no se valida en acceso
La licencia existe como entidad pero no se verifica en el pipeline de autenticación/autorización. Un usuario autenticado puede usar todos los módulos independientemente de la licencia.

### Sin política de roles granular confirmada
El sistema de `Permission` existe en el dominio pero su uso activo en `[Authorize(Policy = "...")]` no está confirmado en los controllers revisados. Todos usan `[Authorize]` sin política específica.

## Base de datos

### Sin índices confirmados en tablas de alto volumen
Las configuraciones EF añaden índices únicos en campos clave (numbers, codes), pero no se verificaron índices de rendimiento en FKs frecuentes (CustomerId en SalesOrders, ItemId en líneas, etc.).

### Datos contables demo escasos
El `DemoDataSeeder` crea escenarios de venta pero no crea ejercicios fiscales, períodos ni asientos. La UI de contabilidad arranca sin datos de referencia (excepto los sembrados vía HasData en la migración).
