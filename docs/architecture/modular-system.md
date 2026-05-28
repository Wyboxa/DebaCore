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
Core       1.0.0  ← implementado
CRM        1.0.0  ← implementado
Suppliers  0.0.0  ← pendiente ERP-1
Catalog    0.0.0  ← pendiente ERP-1
Sales      0.0.0  ← pendiente ERP-2/3
Purchasing 0.0.0  ← pendiente ERP-2/3
Inventory  0.0.0  ← pendiente ERP-4
Accounting 0.0.0  ← pendiente ERP-5
Documents  0.0.0  ← pendiente (backlog)
AI         0.1.0  ← abstracción base, pendiente Fase 4
```

## Estado de módulos

| Módulo      | Estado | Fase | Descripción |
|-------------|--------|------|-------------|
| Core        | ✅ Implementado | Fase 2 | Usuarios, roles, permisos, auditoría |
| CRM         | ✅ Implementado | Fase 3 | Clientes, contactos, actividades, oportunidades |
| AI          | 🔨 Parcial | Fase 4 | Abstracción `IAIProvider` + `MockAIProvider` — sin integración funcional |
| Suppliers   | ⏳ Pendiente | ERP-1 | Proveedores y contactos |
| Catalog     | ⏳ Pendiente | ERP-1 | Artículos, tarifas, IVA |
| Sales       | ⏳ Pendiente | ERP-2/3 | Pedidos, albaranes, facturas de venta |
| Purchasing  | ⏳ Pendiente | ERP-2/3 | Pedidos, albaranes, facturas de compra |
| Inventory   | ⏳ Pendiente | ERP-4 | Almacenes, stock, movimientos |
| Accounting  | ⏳ Pendiente | ERP-5 | Plan contable, asientos, vencimientos |
| Documents   | ⏳ Pendiente | Backlog | Documentos, versiones, plantillas |
