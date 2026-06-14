---
type: module
module: ia
layer: cross
status: implemented
source:
  - src/Debales.AI/
  - src/Debales.Application/AI/
  - src/Debales.Api/Controllers/AIController.cs
  - src/Debales.Web/Components/Pages/IA/IA.razor
related:
  - Customer
  - Supplier
  - SalesInvoice
  - PurchaseInvoice
  - AccountingEntry
---

# Módulo IA (ERP-6)

## Qué problema resuelve

Inteligencia artificial supervisada sobre el ERP: chat financiero con contexto de datos reales, detección de anomalías, resúmenes de cliente/proveedor y briefing del dashboard.

## Estado

Implementado — sin migración propia (usa datos de módulos anteriores).

## Proveedores de IA

| Proveedor | Clase | Activación |
|-----------|-------|------------|
| Claude (Anthropic) | `ClaudeProvider` | `AI__Provider=Claude` en config |
| Mock | `MockAIProvider` | `AI__Provider=Mock` (default dev) |

Selección por configuración en `AISettings`. El modelo por defecto es `claude-sonnet-4-6`.

## Componentes de Application

| Componente | Tipo | Descripción |
|------------|------|-------------|
| `IAIService` | Interface | Contrato del servicio IA |
| `ChatWithERPHandler` | Handler | Chat con contexto ERP (facturas, cobros, pagos, asientos) |
| `GetERPAnomaliesHandler` | Handler | Detecta anomalías financieras |
| `GetCustomerERPSummaryHandler` | Handler | Resumen financiero de un cliente |
| `GetSupplierERPSummaryHandler` | Handler | Resumen financiero de un proveedor |
| `GetDashboardBriefingHandler` | Handler | Briefing general del estado del ERP |
| `ChatWithCustomerHandler` | Handler | Chat con contexto CRM de un cliente |
| `GetCustomerSummaryHandler` | Handler | Resumen CRM de un cliente |
| `ERPAIContext` | Record | Contexto de datos ERP para el prompt |
| `CustomerERPContext` | Record | Datos ERP de un cliente |
| `SupplierAIContext` | Record | Datos ERP de un proveedor |
| `PromptBuilder` | Service | Construcción de prompts enriquecidos |

## Controllers

| Controller | Ruta | Endpoints |
|------------|------|-----------|
| `AIController` | `api/ai` | POST `/customers/{id}/chat`, GET `/customers/{id}/summary` |

## Página Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `IA.razor` | `/ia` | Implementada — 4 tabs: Chat ERP, Anomalías, Análisis cliente, Análisis proveedor |

## Capacidades de la UI

1. **Chat ERP** — Chat libre con sugerencias predefinidas sobre facturas, cobros, pagos y contabilidad
2. **Anomalías** — Análisis automático con severidad Alta/Media/Ok
3. **Análisis cliente** — Resumen financiero por cliente seleccionado
4. **Análisis proveedor** — Resumen financiero por proveedor seleccionado

## Principio de supervisión

La IA solo puede consultar y resumir datos. No puede:
- Crear ni modificar registros
- Ejecutar migraciones
- Desplegar cambios
- Acceder a datos fuera del contexto construido

## Lo que está completo

- Arquitectura de proveedores intercambiables
- Chat ERP con contexto de datos reales
- Detección de anomalías
- Resúmenes cliente y proveedor
- Briefing de dashboard
- UI completa con 4 tabs

## Lo que falta

- Integración del chat ERP con el módulo [[AIGovernance]] para proponer acciones
- RAG sobre documentación interna (`AIKnowledgeBase`)
- Memoria semántica persistente

## Módulo complementario

[[AIGovernance]] — implementado en 2026-06-14. Añade persistencia del ciclo de propuestas IA y base de conocimiento.
