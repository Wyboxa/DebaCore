---
type: flow
module: ia
layer: cross
status: implemented
source:
  - src/Debales.AI/
  - src/Debales.Application/AI/ERP/
  - src/Debales.Web/Components/Pages/IA/IA.razor
related:
  - IA
  - Customer
  - Supplier
  - SalesInvoice
  - PurchaseInvoice
  - AccountingEntry
---

# Flujo: IA Supervisada

## Diagrama

```mermaid
graph TD
    A[Usuario en /ia] -->|Pregunta ERP| B[ChatWithERPHandler]
    A -->|Solicita anomalías| C[GetERPAnomaliesHandler]
    A -->|Selecciona cliente| D[GetCustomerERPSummaryHandler]
    A -->|Selecciona proveedor| E[GetSupplierERPSummaryHandler]
    
    B -->|Construye contexto ERP| F[ERPAIContext\nFacturas + Cobros + Contabilidad]
    C -->|Analiza indicadores| G[ERPAnomalyDto lista]
    D -->|Datos del cliente| H[CustomerERPContext]
    E -->|Datos del proveedor| I[SupplierAIContext]
    
    F -->|Prompt| J[IAIProvider]
    G -->|Prompt| J
    H -->|Prompt| J
    I -->|Prompt| J
    
    J -->|Config Claude| K[ClaudeProvider\nclaude-sonnet-4-6]
    J -->|Config Mock| L[MockAIProvider\nDesarrollo]
    
    K -->|Respuesta texto| M[Usuario ve respuesta]
    L -->|Respuesta fija| M
```

## Principio de supervisión

La IA **no puede modificar datos**. Solo puede:
- Leer contexto construido explícitamente por los handlers
- Generar texto de análisis y recomendaciones
- Detectar patrones en los datos

## Flujo de Chat ERP

1. Usuario escribe pregunta en `/ia` tab "Chat ERP"
2. `ChatWithERPHandler` recopila datos ERP actuales (facturas, cobros, pagos, asientos)
3. Construye `ERPAIContext` con los datos relevantes
4. `PromptBuilder` genera el prompt con contexto
5. `IAIProvider` (Claude o Mock) genera la respuesta
6. Respuesta aparece en el chat como burbuja del asistente

## Flujo de Anomalías

1. Usuario activa tab "Anomalías" o pulsa "Actualizar"
2. `GetERPAnomaliesHandler` analiza indicadores del ERP:
   - Facturas sin contabilizar
   - Vencimientos vencidos
   - Stock bajo mínimos (inferido)
   - Asientos descuadrados (si draft)
3. Devuelve lista de `ERPAnomalyDto` con severidad (Alta/Media/Ok)
4. UI muestra alertas con color según severidad

## Flujo de Análisis cliente/proveedor

1. Usuario selecciona cliente o proveedor en dropdown
2. Handler recopila datos ERP del tercero (facturas, cobros/pagos, pendientes)
3. IA genera resumen financiero en texto libre
4. Texto aparece en card de análisis

## Configuración

```
AI__Provider=Claude    → usa ClaudeProvider (requiere AI__ApiKey)
AI__Provider=Mock      → usa MockAIProvider (sin API key)
AI__Model=claude-sonnet-4-6   → modelo por defecto
```
