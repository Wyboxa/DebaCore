# ADR-0006 — Exclusión de módulos verticales del núcleo del producto

## Estado

Aceptada — 2026-05-28

## Contexto

El producto Debales nace de experiencia con aplicaciones empresariales concretas.
Existe riesgo de que la lógica específica de sectores o clientes contamine el núcleo del producto.

Si el núcleo contiene lógica vertical:
- El producto no puede venderse a otros sectores sin modificación.
- Las actualizaciones del núcleo afectan a personalizaciones de clientes.
- La base de código se fragmenta por cliente en lugar de por módulo.

## Decisión

El núcleo del producto Debales (Core, CRM, Sales, Purchasing, Catalog, Inventory, Accounting)
**no contiene** lógica específica de ningún sector industrial o cliente concreto.

### Elementos explícitamente excluidos del núcleo

- Cálculo de mamparas o elementos similares.
- Escandallos industriales.
- Órdenes y partes de fabricación propios de un sector.
- Partes de consumo de materiales.
- Configuradores de producto industriales.
- Plantillas de documentos específicas de clientes.
- Procesos industriales verticales.
- Cualquier lógica nombrada por el cliente: "como lo hace Empresa X".

### Cómo se maneja la necesidad vertical

Las necesidades sectoriales se implementan como **módulos opcionales**:

```txt
Debales.Modules.[Sector].[Feature]
```

Reglas para módulos verticales:
1. Tienen manifiesto propio con versión y dependencias declaradas.
2. Dependen del núcleo pero el núcleo no depende de ellos.
3. Extienden entidades del núcleo via composición, no modificación.
4. Se activan/desactivan sin afectar otros clientes.
5. Tienen sus propias migraciones, separadas de las del núcleo.
6. Sus tests no pueden fallar por cambios en el núcleo (contratos estables).

## Consecuencias

- El producto puede venderse a cualquier empresa sin modificar el núcleo.
- Los módulos verticales pueden desarrollarse en paralelo sin bloquear el producto base.
- Un cliente con necesidades especiales paga por el módulo vertical, no exige cambios al núcleo.
- El equipo técnico puede distinguir claramente qué es núcleo y qué es personalización.

## Alternativas consideradas

**Alternativa: Núcleo configurable con flags por sector**
- Rechazada. Los flags crecen de forma incontrolable. Complejidad sin modularidad real.

**Alternativa: Fork por cliente**
- Rechazada. Imposible mantener. Cada actualización requiere N merges.
