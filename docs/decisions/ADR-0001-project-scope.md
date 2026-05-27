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
- No se implementan funcionalidades fuera de alcance inicial: contabilidad completa, stock complejo, facturación legal, multi-país, marketplace, microservicios, Kubernetes, BI avanzado.
- Cada fase entrega algo concreto y revisable.

## Alternativas consideradas

- Construir ERP completo desde el inicio: descartado por complejidad y tiempo de validación.
- SaaS puro desde el inicio: descartado porque el diferencial es on-premise/local.
- Microservicios: descartado para el MVP por overhead operativo innecesario.
