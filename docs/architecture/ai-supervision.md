# Gobernanza IA — Debales

## Principio no negociable

La IA no modifica producción directamente.

## Niveles de automatización

| Nivel | Nombre                    | Descripción                                                  |
|-------|---------------------------|--------------------------------------------------------------|
| 0     | Consulta                  | La IA responde dudas                                         |
| 1     | Documentación             | La IA genera docs, resúmenes y propuestas                    |
| 2     | Configuración asistida    | La IA propone cambios de config, no los aplica sin confirmar |
| 3     | Código asistido           | La IA genera ramas, código, migraciones y tests              |
| 4     | Integración supervisada   | La IA prepara pull requests completos                        |
| 5     | Automatización avanzada   | Solo entornos controlados, nunca producción directa          |

**El proyecto empieza en Nivel 1-2.**

## Flujo de cambio

```
Cliente solicita cambio
↓
IA entiende necesidad
↓
IA genera propuesta funcional
↓
Experto funcional revisa
↓
IA genera diseño técnico
↓
Experto técnico revisa
↓
IA prepara implementación
↓
Tests y revisión
↓
Aprobación humana
↓
Despliegue controlado
```

## Abstracción de proveedores IA

```
IAIProvider
├── ClaudeProvider
├── OpenAIProvider
├── AzureOpenAIProvider
├── LocalModelProvider
└── MockAIProvider
```

El dominio no se acopla directamente a ningún proveedor IA concreto.

## Herramientas IA permitidas

| Categoría     | Herramientas                                                       |
|---------------|--------------------------------------------------------------------|
| Lectura       | ReadCustomer, SearchDocuments, ReadModuleDocumentation             |
| Generación    | CreateDraftTask, GenerateChangeProposal, CreateTechnicalPlan       |
| Código        | CreateBranch, GenerateCodePatch, RunTests                          |
| **Peligrosas**| ApplyMigration, WriteProductionData, DeployVersion, DeleteData     |

Las herramientas peligrosas requieren aprobación humana explícita.

## Contexto IA

La IA recibe contexto controlado, no todo:

- Usuario actual y permisos.
- Empresa/Tenant.
- Módulos activos.
- Datos relevantes a la tarea.
- Historial limitado de conversación.
- Documentación aplicable.
