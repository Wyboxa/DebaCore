# Módulo Audit — Debales

## Estado

Implementación básica en Core (AuditEntry existente). Módulo completo pendiente.

## Qué existe hoy

`AuditEntry` en `Debales.Domain.Core.Audit` con:
- `EntityName`, `EntityId`, `Action`, `UserId`
- `OldValues`, `NewValues` (JSON)
- Factory method `Record()`

## Qué falta para el módulo completo

```txt
AuditLog (extensión de AuditEntry actual)
 ├── TenantId (cuando multi-tenant esté activo)
 ├── Module (ej: CRM, Sales, Accounting)
 ├── IpAddress?
 ├── CorrelationId (para trazar una operación completa)
 └── Reason? (motivo del cambio si aplica)
```

## Eventos auditables prioritarios

```txt
Acciones críticas:
- Alta, modificación y baja de usuarios
- Cambios de roles y permisos
- Contabilización de facturas
- Cobros y pagos confirmados
- Cierre de ejercicio/periodo
- Ajustes de stock
- Cambios de configuración del sistema
- Acciones IA relevantes y sus aprobaciones

Acciones de seguimiento:
- Alta y modificación de clientes
- Alta y modificación de artículos
- Creación de documentos (pedidos, albaranes, facturas)
```

## Reglas

- La auditoría nunca se borra.
- Los registros de auditoría son de solo lectura para todos los usuarios.
- La captura de auditoría es automática (via EF Core interceptor o similar), no manual.
- Los campos sensibles (contraseñas, tokens) nunca se loguean en auditoría.
