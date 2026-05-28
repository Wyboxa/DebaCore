# Estado actual del proyecto — Debales

> Auditoría realizada: 2026-05-28  
> Fuente: revisión directa del código en `D:\Debales`

---

## 1. Estructura del proyecto

Solución .NET 8 (`Debales.slnx`) con 10 proyectos organizados en arquitectura limpia:

```
src/
├── Debales.Domain          ← Entidades, value objects, reglas de dominio
├── Debales.Application     ← Casos de uso, handlers, DTOs, interfaces
├── Debales.Infrastructure  ← EF Core, repositorios, persistencia, seguridad
├── Debales.Api             ← API REST (ASP.NET Core)
├── Debales.Web             ← UI Blazor Server
├── Debales.AI              ← Abstracción IA (estructura inicial)
└── Debales.Modules/Core    ← Manifiesto del módulo Core

tests/
├── Debales.Domain.Tests
├── Debales.Application.Tests
└── Debales.Integration.Tests
```

---

## 2. Funcionalidades operativas

### 2.1 Base de datos

- SQL Server LocalDB configurada y migrada.
- 2 migraciones aplicadas: `InitialCreate` (Core) y `AddCrmModule` (CRM).
- 12 tablas en total.
- Configuraciones EF Core completas para todas las entidades.

### 2.2 Módulo Core

**Dominio:**
- `User` con value object `Email`
- `Role`, `Permission`, `RolePermission`, `UserRole`
- `SystemModule`
- `AuditEntry`
- Clase base `AuditableEntity` y `Entity`

**Application:**
- `CreateUserCommand` / `CreateUserHandler`
- `GetUserByIdQuery` / `GetUserByIdHandler`
- `IUserRepository`, `IRepository<T>`, `IUnitOfWork`
- `IPasswordHasher`

**Infrastructure:**
- `UserRepository`, `PasswordHasher`, `UnitOfWork`
- `ApplicationDbContext` con todas las configuraciones

**API:**
- `UsersController` con endpoints básicos
- `HealthController`

**Módulo registrado:**
- `CoreModule` implementa `IModule` con versión `1.0.0` y 9 permisos declarados.

### 2.3 Módulo CRM

**Dominio:**
- `Customer` con `Address` (value object)
- `Contact`
- `Activity` + enum `ActivityType` (Call, Email, Meeting, Task, Other)
- `Note`
- `Opportunity` + enum `OpportunityStatus` (New, InProgress, Won, Lost)

**Application:**
- Customers: `CreateCustomerCommand`, `UpdateCustomerCommand`, `GetCustomersQuery` (paginada), `GetCustomerByIdQuery`
- Contacts: `AddContactCommand`, `GetContactsByCustomerQuery`
- Activities: `LogActivityCommand`, `GetActivitiesByCustomerQuery`
- Notes: `AddNoteCommand`, `GetNotesByCustomerQuery`
- Opportunities: `CreateOpportunityCommand`, `UpdateOpportunityStatusCommand`, `GetOpportunitiesByCustomerQuery`
- DTOs: `CustomerSummaryDto`, `CustomerDetailDto`, `ContactDto`, `ActivityDto`, `NoteDto`, `OpportunityDto`
- `PagedResult<T>` para paginación

**Infrastructure:**
- Repositorios: `CustomerRepository`, `ContactRepository`, `ActivityRepository`, `NoteRepository`, `OpportunityRepository`
- Configuraciones EF Core para todas las entidades CRM

**API REST — 11 endpoints operativos:**

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
| PATCH | `/api/customers/{id}/opportunities/{oppId}/status` | Cambiar estado oportunidad |

**UI Blazor Server — 2 páginas operativas:**

- `/crm/customers` — Lista de clientes con búsqueda, paginación y modal de creación.
- `/crm/customers/{id}` — Ficha con 5 pestañas: Información, Contactos, Actividades, Notas, Oportunidades. CRUD inline en cada pestaña.

### 2.4 Módulo AI

- `IAIProvider` (interfaz con método `CompleteAsync`).
- `MockAIProvider` (implementación mock que devuelve respuesta vacía).
- **Sin integración funcional** con la UI ni con la API.

### 2.5 Tests automatizados

~31 tests distribuidos en:

| Proyecto | Archivos de test |
|----------|-----------------|
| Domain.Tests | EntityTests, UserTests, RoleTests, CustomerTests, OpportunityTests |
| Application.Tests | RepositoryInterfaceTests, CreateUserHandlerTests, CreateCustomerHandlerTests |
| Integration.Tests | SolutionSmokeTests |

### 2.6 Documentación

Estructura `docs/` completa con:
- `docs/architecture/`: 7 documentos (overview, modular-system, ai-supervision, security, deployment, module-boundaries, accounting-foundation)
- `docs/decisions/`: 6 ADRs (ADR-0001 a ADR-0006)
- `docs/modules/`: 8 módulos documentados (core, crm, accounting, ai, audit, catalog, documents, inventory, purchasing, sales)
- `docs/operations/`: git-flow, release-process, testing
- `docs/product/`: vision, business-model, roadmap

---

## 3. Limitaciones y bugs detectados

### 3.1 Sin autenticación (CRÍTICO)

`Program.cs` en `Debales.Api` llama `app.UseAuthorization()` pero **no hay `UseAuthentication()` ni ningún esquema de autenticación configurado**. Todos los endpoints son públicos sin restricción.

La UI Blazor tampoco tiene pantalla de login ni estado de sesión.

**Impacto:** cualquier petición a la API funciona sin credenciales. No puede usarse en producción ni demostración real.

### 3.2 `CreatedBy` hardcodeado

Todos los commands reciben `createdBy` como string literal `"web"` o `"api"` en lugar del usuario autenticado real. La trazabilidad de auditoría es inútil hasta que haya sesión real.

**Archivos afectados:** todos los controllers y páginas Blazor que crean o actualizan entidades.

### 3.3 Sin seed de datos inicial

No existe ningún mecanismo de seed en la infraestructura. Al ejecutar la aplicación por primera vez, la base de datos está vacía: sin usuario administrador, sin roles base, sin módulos registrados.

**Impacto:** la aplicación arranca pero no hay ningún usuario con el que operar.

### 3.4 Sin middleware de gestión de errores global

La API no tiene middleware global de excepciones. Una excepción no controlada expone el stack trace completo al cliente en entornos de desarrollo y puede devolver 500 sin formato coherente en producción.

### 3.5 Sistema modular desconectado en runtime

`CoreModule` implementa `IModule` y declara permisos, pero no existe lógica de activación/desactivación en runtime. La tabla `SystemModules` existe en BD pero no hay servicio que compruebe qué módulos están activos antes de ejecutar operaciones.

### 3.6 Sin endpoint de notas GET

Existe `POST /api/customers/{id}/notes` pero no `GET /api/customers/{id}/notes`. La UI lee las notas directamente desde el handler, pero la API pública carece del endpoint de consulta.

### 3.7 Oportunidades: `customerId` no se valida en `UpdateOpportunityStatus`

El endpoint `PATCH /customers/{customerId}/opportunities/{opportunityId}/status` recibe `customerId` en la ruta pero el handler solo usa `opportunityId`. Un cliente podría cambiar el estado de una oportunidad que no le pertenece si conoce el UUID.

---

## 4. Módulos no implementados

Los siguientes módulos están documentados en `docs/modules/` pero **no tienen código**:

- Proveedores (Supplier)
- Catálogo (Item, Service, PriceList…)
- Ventas (SalesOrder, SalesInvoice…)
- Compras (PurchaseOrder, PurchaseInvoice…)
- Almacén (Warehouse, StockMovement…)
- Contabilidad (AccountingEntry, FiscalYear…)
- Documentos (Document, DocumentTemplate…)
- IA supervisada funcional

---

## 5. Decisiones pendientes (del CLAUDE.md §47.2)

| Tema | Impacto |
|------|---------|
| Multi-tenant desde inicio | Afecta todas las tablas (`TenantId`) |
| Motor contable propio vs librería | Esfuerzo y flexibilidad en Fase ERP-5 |
| Plan contable por defecto (PGC España / Internacional) | Primer cliente objetivo |
| Formas de pago y remesas bancarias | Facturación en Fase ERP-3 |
| Generación automática de asientos | Plantillas vs hardcoded por tipo de documento |
| Cuentas contables individuales por cliente/proveedor | PGC español lo exige |
