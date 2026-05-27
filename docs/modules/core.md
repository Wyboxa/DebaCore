# Módulo Core — Debales

## Estado

Planificado — Fase 2

## Dependencias

Ninguna. Es el módulo base obligatorio.

## Propósito

Proporcionar la estructura empresarial mínima sobre la que se apoyan todos los demás módulos.

## Funcionalidades previstas

### Usuarios

- Alta, edición y desactivación de usuarios.
- Autenticación (local, con posibilidad de SSO en futuro).
- Contraseña segura y recuperación.

### Roles y permisos

- Roles configurables por instalación.
- Permisos granulares por módulo y acción (ej: `crm.customers.read`).
- Asignación de roles a usuarios.

### Módulos

- Registro de módulos disponibles e instalados.
- Activación/desactivación por cliente.
- Versión de cada módulo.

### Menú dinámico

- Menú generado según módulos activos y permisos del usuario.

### Auditoría básica

- Registro de acciones críticas: altas, modificaciones, eliminaciones, cambios de configuración.

### Configuración

- Parámetros de la instalación (nombre empresa, zona horaria, idioma).

## Tablas principales (propuesta)

```
SystemUsers
SystemRoles
SystemPermissions
SystemRolePermissions
SystemModules
SystemAuditLog
SystemSettings
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
