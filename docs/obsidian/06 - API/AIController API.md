---
type: api
module: ia
layer: api
status: implemented
source:
  - src/Debales.Api/Controllers/AIController.cs
related:
  - IA
  - Customer
---

# AIController

**Ruta base**: `api/ai`  
**Autorización**: JWT Bearer requerido

## Endpoints

| Método | Ruta | Handler | Descripción |
|--------|------|---------|-------------|
| POST | `/customers/{id}/chat` | `ChatWithCustomerHandler` | Chat IA con contexto CRM del cliente |
| GET | `/customers/{id}/summary` | `GetCustomerSummaryHandler` | Resumen CRM del cliente |

## Request body (POST chat)

```csharp
record ChatRequest(IReadOnlyList<ChatMessage> History, string Message)
```

## Respuestas de error

| Error | Código HTTP |
|-------|-------------|
| Cliente no encontrado | 404 |
| Error de validación | 400 (con campo `error`) |

## Nota

Los handlers de IA del ERP-6 (`ChatWithERPHandler`, `GetERPAnomaliesHandler`, etc.) **no tienen endpoints API propios**. Solo están disponibles desde la UI Blazor directamente. El AIController solo expone las funcionalidades de IA del CRM (chat y resumen por cliente).
