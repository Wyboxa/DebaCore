---
type: module
module: core
layer: cross
status: implemented
source:
  - src/Debales.Domain/Core/
  - src/Debales.Application/Core/
  - src/Debales.Infrastructure/Persistence/Repositories/UserRepository.cs
  - src/Debales.Infrastructure/Security/
  - src/Debales.Api/Controllers/AuthController.cs
  - src/Debales.Api/Controllers/UsersController.cs
related:
  - User
  - Role
  - Permission
  - AuditEntry
  - SystemModule
  - DbContext
---

# Módulo Core

## Qué problema resuelve

Fundamento de la plataforma: identidad, roles, permisos y auditoría. Todo módulo depende de Core.

## Estado

Implementado — migración `InitialCreate` (2026-05-27).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[User]] | Usuario de la plataforma con email único y roles asignados |
| [[Role]] | Rol con nombre único y flag `isSystem` |
| [[Permission]] | Permiso atómico (no confirmado su uso activo en código) |
| [[RolePermission]] | Relación N:M entre Role y Permission |
| [[UserRole]] | Relación N:M entre User y Role |
| [[AuditEntry]] | Registro de auditoría |
| [[SystemModule]] | Registro de módulo instalado |

## DTOs

No hay un archivo de DTOs de Core unificado — los DTOs de Auth y Users están en sus respectivas carpetas de Application.

## Handlers

| Handler | Tipo | Descripción |
|---------|------|-------------|
| `LoginHandler` | Command | Autentica usuario, devuelve JWT |
| `CreateUserHandler` | Command | Crea usuario con hash de contraseña |
| `GetUserByIdHandler` | Query | Obtiene usuario por ID |

## Controllers

| Controller | Ruta base | Endpoints |
|------------|-----------|-----------|
| `AuthController` | `api/auth` | POST /login |
| `UsersController` | `api/users` | POST /, GET /{id} |
| `HealthController` | `api/health` | GET / |

## Páginas Blazor

No hay páginas Blazor específicas de Core (Login está en `/login` pero no es parte del módulo funcional).

## Repositorios

- `IUserRepository` → `UserRepository`

## Servicios de seguridad

- `IPasswordHasher` → `PasswordHasher`
- `ITokenService` → `JwtTokenService`

## Lo que está completo

- Creación y autenticación de usuarios con JWT
- Roles del sistema (Admin, User) sembrados en startup
- Usuario admin sembrado: `admin@debales.local` / `Admin1234!`
- Hash de contraseñas

## Lo que falta

- Gestión de permisos por rol desde UI
- Páginas de administración de usuarios en Blazor
- Auditoría activa (tabla `AuditEntries` existe pero no se siembra y su uso activo no está confirmado)
