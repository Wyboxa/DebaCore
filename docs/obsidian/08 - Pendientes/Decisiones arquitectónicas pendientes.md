---
type: audit
module: cross
layer: cross
status: pending
related:
  - Pendientes priorizados
  - Contradicciones detectadas
---

# Decisiones arquitectónicas pendientes

Tomadas de CLAUDE.md §47.2 y ampliadas con hallazgos de la auditoría.

## De CLAUDE.md §47.2

| Tema | Opciones | Impacto |
|------|----------|---------|
| Multi-tenant | Sí ahora / No hasta Fase ERP | Afecta todas las tablas (TenantId) |
| Motor contable propio vs librería | Implementación propia / Adaptar librería .NET | Esfuerzo y flexibilidad |
| Plan contable por defecto | PGC España / Internacional / Configurable | Primer cliente objetivo |
| Formas de pago y remesas bancarias | SEPA / Genérico configurable | Facturación |
| Generación automática de asientos | Plantillas vs Hardcoded | Flexibilidad futura |
| Cuentas contables por tercero | Cuenta única / Cuenta individual | PGC español exige individual |

## Detectadas en auditoría

### ¿Middleware de licencia?
¿Se valida la licencia en el pipeline HTTP para restringir acceso según módulos contratados? Actualmente no está implementado.

### ¿Handlers de IA con endpoint API?
Los handlers de IA del ERP-6 solo se consumen desde la UI Blazor. ¿Deben tener endpoints REST para integración externa?

### ¿Albaranes generan movimientos de stock?
No está claro si es un requisito la integración automática de albaranes con el módulo de inventario. Si se decide que sí, hay que definir si el albarán de venta genera salida y el de compra entrada.

### ¿Mediator o sin mediator?
El proyecto usa CQRS manual sin MediatR. Si el número de handlers sigue creciendo, puede ser necesario evaluar la adopción de MediatR para reducir el boilerplate de DI.

### ¿Sin CI/CD?
No se encontró configuración de CI/CD (.github/workflows, azure-pipelines.yml, etc.). Para un producto empresarial, esto es una decisión que hay que tomar.
