# Límites de módulos — Debales

## Principio

Cada módulo es una isla funcional con contratos explícitos hacia los demás.

```txt
Módulo A  →  evento / interfaz  →  Módulo B
```

Un módulo nunca llama directamente a entidades internas de otro módulo.

## Grafo de dependencias

```txt
Core (sin dependencias externas)
 ├── CRM         (depende de Core)
 ├── Catalog     (depende de Core)
 ├── Suppliers   (depende de Core)
 ├── Sales       (depende de Core, CRM, Catalog)
 ├── Purchasing  (depende de Core, Suppliers, Catalog)
 ├── Inventory   (depende de Core, Catalog)
 ├── Accounting  (depende de Core; recibe eventos de Sales, Purchasing)
 ├── Documents   (depende de Core)
 └── AI          (depende de todos; nunca es dependencia de nadie)
```

**Regla:** Accounting no importa nada de Sales ni Purchasing. Solo consume sus eventos.

## Módulos verticales (opcionales)

```txt
Debales.Modules.[Sector].[Feature]
 └── depende de módulos núcleo (via interfaces, no implementaciones)
 └── el núcleo NO depende del módulo vertical
```

Ver: [ADR-0006](../decisions/ADR-0006-vertical-modules-exclusion.md)

## Contratos entre módulos

Los módulos se comunican via:

1. **Eventos de dominio**: un módulo publica, otro consume. Desacoplamiento máximo.
2. **Interfaces declaradas en Application**: el módulo B declara la interfaz, el módulo A la implementa.
3. **DTOs**: nunca pasar entidades de dominio entre módulos.

## Activación de módulos

Cada módulo puede estar activo o inactivo por instalación/tenant.

```txt
GET /api/modules          → lista módulos activos
POST /api/modules/enable  → activa módulo (requiere permiso)
```

Si un módulo está inactivo, sus endpoints retornan 404 o 403 según política.
Las migraciones de un módulo inactivo no se aplican.

## Tabla resumen de módulos

| Módulo | Depende de | Descripción |
|---|---|---|
| Core | — | Usuarios, roles, permisos, configuración |
| CRM | Core | Clientes, contactos, actividades |
| Catalog | Core | Artículos, servicios, tarifas, IVA |
| Suppliers | Core | Proveedores, contactos |
| Sales | Core, CRM, Catalog | Presupuestos, pedidos, albaranes, facturas |
| Purchasing | Core, Suppliers, Catalog | Pedidos, albaranes, facturas de compra |
| Inventory | Core, Catalog | Almacenes, stock, movimientos |
| Accounting | Core | Plan contable, asientos, vencimientos |
| Documents | Core | Documentos, adjuntos, plantillas |
| AI | Core + todos | Contexto IA, propuestas, aprobaciones |
