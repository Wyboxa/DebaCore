# ADR-0003 — Gobernanza de IA

## Estado

Aceptada

## Contexto

El producto tiene IA integrada tanto en el plano cliente (ayuda funcional) como en el plano técnico interno (asistencia al desarrollo). Sin una política clara, la IA puede causar cambios destructivos, mezclar datos de clientes o actuar sin supervisión.

## Decisión

### Principio no negociable

La IA no modifica producción directamente. Toda acción relevante requiere aprobación humana explícita.

### Niveles de automatización

El proyecto empieza en **Nivel 1-2**:

- Nivel 1: La IA genera documentación, resúmenes y propuestas.
- Nivel 2: La IA propone cambios de configuración pero no los aplica sin confirmación.

Los niveles 3-5 se introducen progresivamente con supervisión y pruebas.

### Separación de contexto

- La IA recibe contexto controlado por tarea, no acceso global.
- No se mezclan datos entre clientes/tenants.
- No se envían datos del cliente a proveedores externos sin consentimiento.

### Herramientas con aprobación obligatoria

`ApplyMigration`, `WriteProductionData`, `DeployVersion`, `DeleteData`, `ChangeLicense`

## Consecuencias

- Cada propuesta de cambio generada por IA pasa por revisión funcional y técnica antes de implementarse.
- Se registran en auditoría las acciones IA relevantes y sus aprobaciones.
- El proveedor IA es intercambiable sin modificar el dominio.

## Alternativas consideradas

- IA con acceso total: descartado por riesgo legal, de privacidad y de datos.
- Sin IA en el MVP: descartado porque es el diferencial central del producto.
- Proveedor IA acoplado directamente: descartado por bloqueo de vendor y dificultad de testing con MockAIProvider.
