# Módulo Core — Debales

## Estado

**Implementado — Fase 2 completada**

## Dependencias

Ninguna. Es el módulo base obligatorio.

## Propósito

Proporcionar la estructura empresarial mínima sobre la que se apoyan todos los demás módulos.

## Implementado

### Dominio (`Debales.Domain/Core/`)

- `User` con value object `Email`
- `Role`, `Permission`, `RolePermission`, `UserRole`
- `SystemModule`
- `AuditEntry` con factory method `Record()`
- Clase base `AuditableEntity` y `Entity`

### Application (`Debales.Application/Core/`)

- `CreateUserCommand` / `CreateUserHandler`
- `GetUserByIdQuery` / `GetUserByIdHandler`
- Interfaces: `IUserRepository`, `IRepository<T>`, `IUnitOfWork`, `IPasswordHasher`

### Infrastructure (`Debales.Infrastructure/`)

- `UserRepository`, `PasswordHasher`, `UnitOfWork`
- `ApplicationDbContext` con todas las configuraciones EF Core

### API

- `UsersController` con endpoints básicos
- `HealthController`

### Módulo registrado

- `CoreModule` implementa `IModule` con versión `1.0.0` y 9 permisos declarados

## Tablas existentes en BD

```
Users
Roles
Permissions
RolePermissions
UserRoles
SystemModules
AuditEntries
```

## Permisos del módulo

```
core.users.read
core.users.write
core.roles.read
core.roles.write
core.modules.read
core.modules.write
core.settings.read
core.settings.write
core.audit.read
```

## Pendiente (P0 — bloquea uso real)

- Sin autenticación implementada. Todos los endpoints son públicos.
- `CreatedBy` hardcodeado como `"api"` — sin usuario real de sesión.
- Sin seed inicial: BD arranca vacía, sin usuario admin ni roles base.
- Sistema modular desconectado en runtime: `SystemModules` existe pero no hay lógica de activación.

Ver detalle en `estado_actual.md §3` y `roadmap.md — Prioridad 0`.
