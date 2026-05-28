# ADR-0001 — Alcance del proyecto y enfoque MVP

## Estado

Aceptada

## Contexto

El proyecto Debales es una plataforma CRM/ERP modular con IA integrada. El riesgo principal al inicio es el sobrealcance: intentar construir CRM, ERP completo, IA avanzada, licencias, BI y automatización completa desde el primer día.

## Decisión

Construir un MVP real y vendible en fases claras, empezando por los módulos de menor complejidad y mayor valor demostrable:

1. Fase 0: Cimientos (repo, docs, arquitectura).
2. Fase 1: Solución .NET base compilable.
3. Fase 2: Módulo Core (usuarios, roles, permisos).
4. Fase 3: Módulo CRM (primer módulo funcional real).
5. Fases posteriores: IA, licencias, despliegue local.

CRM se prioriza sobre ERP porque tiene menor complejidad, es más demostrable y es útil para cualquier empresa.

## Consecuencias

- El proyecto tiene fases claras y no salta entre ellas sin motivo.
- Cada fase entrega algo concreto y revisable.
- Microservicios, Kubernetes, BI avanzado, multi-país, marketplace: fuera de alcance del núcleo.

## Extensión de alcance — 2026-05-28

Tras completar Fases 0–3, el alcance se extendió con un roadmap ERP en fases progresivas:

- **ERP-1:** Proveedores y catálogo base
- **ERP-2:** Ventas y compras operativas (sin contabilizar)
- **ERP-3:** Facturación, vencimientos, cobros/pagos
- **ERP-4:** Almacén y stock
- **ERP-5:** Contabilidad mínima (asientos automáticos, cierre de ejercicio)
- **ERP-6:** IA sobre ERP

Esta extensión no contradice la decisión original de MVP por fases; la amplía de forma ordenada.
La contabilidad incluida en ERP-5 es **mínima y supervisada**, no un módulo contable completo.

Ver: [ADR-0004](ADR-0004-operational-vs-accounting-documents.md), [ADR-0005](ADR-0005-accounting-events.md), `roadmap.md` (raíz).

## Alternativas consideradas

- Construir ERP completo desde el inicio: descartado por complejidad y tiempo de validación.
- SaaS puro desde el inicio: descartado porque el diferencial es on-premise/local.
- Microservicios: descartado para el MVP por overhead operativo innecesario.
