---
type: module
module: ai-governance
layer: cross
status: implemented
source:
  - src/Debales.Domain/AI/
  - src/Debales.Application/AIGovernance/
  - src/Debales.Infrastructure/Persistence/Configurations/AI/
  - src/Debales.Infrastructure/Persistence/Repositories/AI/
  - src/Debales.Api/Controllers/AIGovernanceController.cs
  - src/Debales.Web/Components/Pages/AI/
  - src/Debales.Web/Components/Pages/Configuracion/
related:
  - IA
  - AIActionProposal
  - AIExecutionLog
---

# Módulo AI Governance

## Qué problema resuelve

Implementa el flujo de supervisión humana sobre propuestas de la IA: registro de reglas (qué acciones requieren aprobación), base de conocimiento, propuestas de acción con ciclo de vida (Pending → Approved/Rejected → Executed/Cancelled) y log de ejecuciones.

Complementa al [[IA|Módulo IA (ERP-6)]] añadiendo persistencia del ciclo de propuestas.

## Estado

Implementado — migración manual `20260614090000_AddAIGovernanceModule`.

## Entidades del dominio

| Entidad | Base | Descripción |
|---------|------|-------------|
| `AIRule` | `AuditableEntity` | Regla: qué tipo de acción requiere aprobación humana |
| `AIKnowledgeBase` | `AuditableEntity` | Entrada de base de conocimiento (título, categoría, contenido largo) |
| `AIActionProposal` | `AuditableEntity` | Propuesta de acción con máquina de estados |
| `AIActionApproval` | `Entity` | Registro inmutable de una revisión (aprobación o rechazo) |
| `AIExecutionLog` | `Entity` | Registro inmutable de una ejecución de acción IA |

## Máquina de estados de `AIActionProposal`

```
Pending → Approved → Executed
        → Rejected → Cancelled
        → Cancelled
Approved → Cancelled
```

Transiciones permitidas:
- `Approve()` — solo desde `Pending`
- `Reject(reason)` — solo desde `Pending`
- `MarkExecuted()` — solo desde `Approved`
- `Cancel()` — desde cualquier estado excepto `Executed` o `Cancelled`

## Handlers de Application (13)

| Handler | Acción |
|---------|--------|
| `CreateAIRuleHandler` | Crea regla IA |
| `UpdateAIRuleHandler` | Actualiza regla IA |
| `CreateAIKnowledgeBaseHandler` | Crea entrada de base de conocimiento |
| `UpdateAIKnowledgeBaseHandler` | Actualiza entrada de base de conocimiento |
| `CreateAIActionProposalHandler` | Crea propuesta en estado Pending |
| `ApproveAIActionProposalHandler` | Aprueba propuesta + crea `AIActionApproval` |
| `RejectAIActionProposalHandler` | Rechaza propuesta + crea `AIActionApproval` |
| `CreateAIExecutionLogHandler` | Registra una ejecución |
| `GetAIRulesHandler` | Lista todas las reglas activas |
| `GetAIKnowledgeBasesHandler` | Lista paginada con búsqueda |
| `GetAIActionProposalsHandler` | Lista paginada filtrada por estado |
| `GetAIActionProposalByIdHandler` | Propuesta + historial de aprobaciones |
| `GetAIExecutionLogsHandler` | Lista paginada de logs de ejecución |

## Controller de API

| Ruta | Métodos | Descripción |
|------|---------|-------------|
| `/api/ai/rules` | GET, POST | Reglas IA |
| `/api/ai/rules/{id}` | PUT | Actualizar regla |
| `/api/ai/knowledge` | GET, POST | Base de conocimiento |
| `/api/ai/knowledge/{id}` | PUT | Actualizar entrada |
| `/api/ai/proposals` | GET, POST | Propuestas |
| `/api/ai/proposals/{id}` | GET | Detalle con historial |
| `/api/ai/proposals/{id}/approve` | POST | Aprobar |
| `/api/ai/proposals/{id}/reject` | POST | Rechazar |
| `/api/ai/execution-logs` | GET, POST | Logs de ejecución |

## Páginas Blazor

| Página | Ruta | Descripción |
|--------|------|-------------|
| `Propuestas.razor` | `/ai/propuestas` | Lista filtrada por estado + aprobar/rechazar rápido |
| `PropuestaDetalle.razor` | `/ai/propuestas/{id}` | Detalle con payload + historial de revisiones |
| `AIReglas.razor` | `/configuracion/ai-reglas` | CRUD reglas IA |
| `AIConocimiento.razor` | `/configuracion/ai-conocimiento` | CRUD base de conocimiento con búsqueda |

## NavMenu

- Sección principal: "Propuestas IA" (enlace `/ai/propuestas`)
- Configuración: "Reglas IA" + "Conocimiento IA"

## Principio de supervisión

`AIActionApproval` y `AIExecutionLog` son entidades inmutables (`Entity`, sin soft-delete): representan el registro histórico de decisiones humanas. No se borran, solo se acumulan.

## Lo que está completo

- Ciclo de vida completo de propuestas con trazabilidad de aprobaciones
- Base de conocimiento editable desde UI
- Reglas configurables de qué requiere aprobación
- Log de ejecuciones

## Lo que falta

- Integración entre `AIController` (chat ERP) y `AIActionProposalHandler` (cuando el chat genere una acción)
- `AIContext` persistente como tabla (actualmente es un objeto en memoria)
- RAG sobre `AIKnowledgeBase`
