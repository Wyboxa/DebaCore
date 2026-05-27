# Módulo AI — Debales

## Estado

Planificado — Fase 4

## Dependencias

- Core 1.0.0
- CRM 1.0.0

## Propósito

Módulo transversal que expone las capacidades de IA supervisada al usuario final.

## Funcionalidades previstas

### Chat con contexto controlado

- Interfaz de conversación con la IA.
- Contexto limitado a: usuario, empresa, módulos activos, datos relevantes.
- Historial de conversación por sesión.

### Resumen de cliente

- La IA genera un resumen ejecutivo del cliente (actividades recientes, oportunidades, notas).

### Explicación de módulos

- La IA explica funcionalidades disponibles según módulos contratados.

### Generación de tareas

- La IA propone tareas o actividades basadas en el contexto del cliente.

### Propuestas de mejora

- La IA detecta necesidades y genera una propuesta estructurada.
- La propuesta requiere validación humana antes de implementarse.

### Registro de conversaciones relevantes

- Las conversaciones marcadas como relevantes quedan guardadas y trazables.

## Reglas de gobernanza

- La IA recibe contexto controlado, no acceso total a la BD.
- No mezcla datos entre clientes.
- No ejecuta acciones destructivas sin aprobación.
- Toda propuesta generada por IA incluye sección "Requiere validación humana: Sí".

## Permisos del módulo

```
ai.chat.use
ai.summaries.read
ai.proposals.read
ai.proposals.approve
ai.conversations.read
```
