# Sistema modular — Debales

## Concepto

Cada módulo es una unidad funcional independiente que puede:

- Activarse y desactivarse por cliente/instalación.
- Versionarse de forma independiente.
- Declarar sus dependencias.
- Registrar permisos, menús, entidades y migraciones propias.
- Exponer capacidades a la IA.

## Manifiesto de módulo

Cada módulo declara un manifiesto:

```json
{
  "name": "CRM",
  "version": "1.0.0",
  "enabled": true,
  "dependencies": ["Core"],
  "permissions": [
    "crm.customers.read",
    "crm.customers.write",
    "crm.contacts.read",
    "crm.contacts.write"
  ],
  "features": [
    "customers",
    "contacts",
    "opportunities",
    "activities"
  ]
}
```

## Tablas de sistema

Propuesta inicial. No crear todas en el MVP — priorizar las mínimas funcionales.

```
SystemTenants
SystemUsers
SystemRoles
SystemPermissions
SystemRolePermissions
SystemModules
SystemModuleVersions
SystemFeatureFlags
SystemAuditLog
SystemSettings
SystemLicenses
```

## Versionado por módulo

Usar SemVer (`MAJOR.MINOR.PATCH`):

```
Core       1.0.0
CRM        1.0.0
Documents  1.0.0
AI         1.0.0
```

## Módulos del MVP

| Módulo    | Estado        | Descripción                          |
|-----------|---------------|--------------------------------------|
| Core      | Obligatorio   | Usuarios, roles, permisos, auditoría |
| CRM       | Fase 3        | Clientes, contactos, actividades     |
| Documents | Fase 3        | Documentos vinculados a clientes     |
| AI        | Fase 4        | Chat, resúmenes, propuestas          |
