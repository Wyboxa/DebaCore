---
type: index
module: cross
layer: cross
status: implemented
related:
  - 00 - Inicio
---

# Cómo usar este vault de Obsidian

## Estructura general

```
docs/obsidian/
  00 - Inicio.md              ← Punto de entrada, estado del proyecto
  01 - Arquitectura.md        ← Capas, dependencias, flujo técnico
  02 - Módulos/               ← Un archivo por módulo funcional
  03 - Entidades/             ← Un archivo por entidad de dominio
  04 - Flujos/                ← Flujos end-to-end con diagramas
  05 - Base de datos/         ← DbContext, migraciones, seeds, tablas
  06 - API/                   ← Controllers y endpoints
  07 - UI Blazor/             ← Páginas Razor organizadas por módulo
  08 - Pendientes/            ← Deuda técnica, contradicciones, huecos
  09 - Diagramas/             ← Diagramas Mermaid de arquitectura
  10 - Auditoría/             ← Inventario completo de artefactos
```

## Convenciones de frontmatter

Cada nota incluye un bloque YAML con:

- `type`: `entity | module | flow | api | ui | database | diagram | audit | index`
- `module`: módulo al que pertenece
- `layer`: capa arquitectónica
- `status`:
  - `implemented` — existe y funciona en código
  - `partial` — existe pero incompleto
  - `placeholder` — página existe pero sin lógica real
  - `inferred` — deducido por contexto, no confirmado directamente
  - `pending` — declarado en CLAUDE.md pero no en código
  - `not_confirmed` — no se pudo verificar
- `source`: archivo(s) de referencia
- `related`: notas relacionadas (usar nombres sin extensión)

## Cómo navegar

- Los `[[enlaces]]` internos conectan notas relacionadas
- Usa el **grafo de Obsidian** para ver relaciones entre módulos y entidades
- Empieza por [[00 - Inicio]] para el estado del proyecto
- Para cada módulo, la nota correspondiente en `02 - Módulos/` referencia todas sus entidades, handlers y páginas

## Sobre los diagramas

Los diagramas están en bloques de código Mermaid. Obsidian los renderiza automáticamente si tienes habilitado el plugin de Mermaid (activo por defecto).

## Sobre las convenciones de auditoría

- **Relación explícita**: hay FK o propiedad de navegación EF confirmada en código
- **Relación inferida**: se deduce por nombre o uso en handler
- Las contradicciones entre CLAUDE.md y código están en [[Contradicciones detectadas]]

## Actualización del vault

Este vault se generó mediante auditoría del repositorio en la fecha `2026-06-04`. Para mantenerlo actualizado, re-ejecutar la auditoría después de cambios significativos en el dominio o arquitectura.
