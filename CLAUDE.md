# CLAUDE.md

## Proyecto: CRM/ERP Modular con Inteligencia Artificial Supervisada

Este archivo define los cimientos, reglas, límites y forma de trabajo del proyecto. Claude Code debe leerlo como la referencia principal de comportamiento dentro de este repositorio.

El objetivo no es crear código rápido. El objetivo es construir una plataforma empresarial modular, fiable, versionada y extensible, donde la IA ayude a analizar, proponer, documentar e implementar cambios bajo supervisión humana.

---

# 1. Identidad del proyecto

## 1.1 Visión

Construir una plataforma CRM/ERP modular con inteligencia artificial integrada, orientada a empresas que necesitan una aplicación adaptable a sus procesos internos.

La plataforma debe permitir que cada empresa tenga módulos activables, datos locales, control de versiones, trazabilidad de cambios y una IA capaz de entender el funcionamiento de la aplicación para ayudar al cliente y a los expertos técnicos.

## 1.2 Idea central

La empresa proveedora no vende únicamente un CRM/ERP.

Vende una plataforma viva donde:

1. El cliente trabaja con la aplicación.
2. El cliente consulta necesidades, dudas o mejoras a una IA.
3. La IA analiza el contexto funcional y técnico.
4. La IA genera una propuesta viable.
5. Un experto humano valida la propuesta.
6. La IA puede preparar código, migraciones, documentación y tests.
7. El equipo técnico revisa, aprueba e integra el cambio.
8. El cambio se versiona y se despliega de forma controlada.

## 1.3 Diferencial del producto

El valor diferencial será:

- CRM/ERP modular.
- IA conocedora de los módulos contratados.
- IA orientada a negocio, no solo a programación.
- Instalación local/on-premise cuando el cliente lo requiera.
- Suscripción por módulos, usuarios y capacidades IA.
- Supervisión humana obligatoria.
- Versionado completo.
- Documentación viva.
- Capacidad de adaptar la aplicación progresivamente a cada empresa.

---

# 2. Principio no negociable

La IA no modifica producción directamente.

La IA puede:

- Analizar.
- Preguntar.
- Documentar.
- Proponer.
- Diseñar.
- Generar código.
- Generar migraciones.
- Crear tests.
- Crear ramas.
- Preparar documentación.
- Explicar impacto.
- Sugerir alternativas.

La IA no puede:

- Desplegar en producción sin aprobación humana.
- Cambiar datos reales sin aprobación explícita.
- Ejecutar migraciones destructivas sin revisión.
- Inventar archivos, clases, endpoints, tablas o decisiones.
- Dar por implementado algo que no ha comprobado.
- Saltarse tests porque “parece correcto”.
- Ocultar incertidumbre.
- Forzar una arquitectura no decidida.

---

# 3. Idioma y estilo de trabajo

## 3.1 Idioma

Responder en español por defecto.

Usar inglés solo en:

- Nombres técnicos estándar.
- Código.
- Commits si el proyecto lo decide.
- Identificadores de clases, métodos, namespaces, tablas o comandos.

## 3.2 Estilo

Ser claro, directo y técnico.

Evitar:

- Respuestas vagas.
- Optimismo falso.
- Explicaciones innecesariamente largas.
- Inventar contexto.
- Decir “está hecho” sin haberlo comprobado.
- Crear capas, patrones o abstracciones sin justificar.

Preferir:

- Pasos accionables.
- Decisiones explícitas.
- Riesgos claros.
- Alternativas razonadas.
- Código pequeño y verificable.
- Documentación útil.
- MVP antes que perfección.

---

# 4. Fuentes de verdad del proyecto

Claude debe respetar esta jerarquía:

1. Código existente en el repositorio.
2. Documentación existente en `/docs`.
3. Decisiones registradas en `/docs/decisions`.
4. Esquemas de base de datos y migraciones existentes.
5. Tests existentes.
6. Este `CLAUDE.md`.
7. Instrucciones concretas del usuario en la conversación actual.
8. Supuestos explícitos, marcados claramente como supuestos.

Si hay contradicción entre fuentes, Claude debe indicarlo y pedir decisión o proponer una resolución.

---

# 5. Protocolo anti-invención

Claude debe seguir este protocolo siempre.

## 5.1 Antes de afirmar que algo existe

Comprobarlo en el repositorio.

No afirmar que existe:

- Una clase.
- Un método.
- Una tabla.
- Una migración.
- Un endpoint.
- Un módulo.
- Un servicio.
- Un patrón.
- Una configuración.
- Un fichero.
- Una carpeta.

Si no se ha localizado, usar una frase clara:

> No he encontrado todavía esa pieza en el repositorio. Puedo proponerla como nueva, pero no asumiré que ya existe.

## 5.2 Si falta información

No inventar.

Hacer una de estas cosas:

1. Buscar en el repositorio.
2. Revisar documentación.
3. Marcarlo como supuesto.
4. Proponer una opción segura.
5. Preguntar solo si es imprescindible.

## 5.3 Formato obligatorio para supuestos

Cuando se necesite asumir algo, escribir:

```md
Supuesto:
- Asumo que [X] porque [motivo].
- Riesgo si este supuesto es falso: [riesgo].
- Cómo validarlo: [acción].
```

## 5.4 Nunca usar nombres inventados como si fueran reales

Si se propone una clase nueva, debe indicarse como propuesta:

Correcto:

```txt
Propongo crear `ModuleRegistryService`.
```

Incorrecto:

```txt
Usa `ModuleRegistryService` como ya está montado.
```

## 5.5 No simular comprobaciones

Prohibido decir:

- “Los tests pasan” si no se han ejecutado.
- “Compila correctamente” si no se ha compilado.
- “La migración funciona” si no se ha probado.
- “No rompe nada” si no se ha revisado impacto.
- “Está integrado” si solo se ha escrito código.

Usar:

```txt
No he ejecutado tests todavía.
```

o

```txt
He ejecutado `dotnet test` y el resultado ha sido...
```

---

# 6. Estado inicial del proyecto

El proyecto está en fase de definición y cimentación.

No asumir que ya existen:

- Solución .NET.
- Base de datos.
- Módulos.
- Agentes.
- Licenciamiento.
- Despliegue.
- UI.
- API.
- Infraestructura.

Hasta que se creen, todo debe tratarse como diseño/propuesta.

---

# 7. Stack tecnológico preferente

## 7.1 Backend

Preferencia inicial:

- .NET.
- C#.
- ASP.NET Core.
- Arquitectura limpia por capas.
- API modular.
- Entity Framework Core o Dapper según necesidad.
- Tests automatizados.

No introducir microservicios al inicio salvo justificación fuerte.

## 7.2 Frontend

Opciones válidas:

- Blazor.
- React.
- Angular.

Para MVP, priorizar simplicidad.

Si se usa Blazor:

- Mantener lógica de negocio fuera de componentes.
- Evitar componentes gigantes.
- Separar servicios, modelos y UI.

Si se usa React:

- Mantener estructura clara.
- Evitar sobreingeniería.
- Usar componentes pequeños.

## 7.3 Base de datos

Opciones preferentes:

- SQL Server.
- PostgreSQL.

Para Carlos, SQL Server es una opción natural por experiencia previa.

Reglas:

- Toda evolución de BD debe ir mediante migraciones.
- Toda tabla crítica debe tener auditoría o trazabilidad si aplica.
- Toda entidad multiempresa debe contemplar `TenantId` o equivalente cuando se introduzca multi-tenant.
- No borrar datos sin estrategia de migración segura.

## 7.4 IA

La IA debe estar aislada detrás de una abstracción.

Propuesta de proveedores:

```txt
IAIProvider
├── ClaudeProvider
├── OpenAIProvider
├── AzureOpenAIProvider
├── LocalModelProvider
└── MockAIProvider
```

No acoplar el dominio directamente a un proveedor IA concreto.

## 7.5 Despliegue

Escenarios previstos:

1. Local/on-premise.
2. Servidor privado del cliente.
3. Nube privada.
4. SaaS gestionado por la empresa proveedora.

El diseño debe permitir evolución, no bloquearse a un único modelo.

---

# 8. Arquitectura base propuesta

## 8.1 Capas iniciales

Estructura recomendada:

```txt
src/
├── ModularAIERP.Api/
├── ModularAIERP.Web/
├── ModularAIERP.Application/
├── ModularAIERP.Domain/
├── ModularAIERP.Infrastructure/
├── ModularAIERP.AI/
└── ModularAIERP.Modules/
    ├── Core/
    ├── CRM/
    ├── ERP/
    ├── Documents/
    └── Billing/
```

## 8.2 Responsabilidades

### Domain

Contiene:

- Entidades de negocio.
- Value Objects.
- Reglas de dominio puras.
- Eventos de dominio.
- Contratos base si aplica.

No debe contener:

- Acceso a base de datos.
- Código de UI.
- Llamadas HTTP.
- Llamadas directas a IA.
- Dependencias de infraestructura.

### Application

Contiene:

- Casos de uso.
- Servicios de aplicación.
- DTOs.
- Validaciones de comandos.
- Orquestación.
- Interfaces hacia infraestructura.

No debe contener:

- SQL directo salvo decisión explícita.
- Lógica visual.
- Código específico de proveedor IA.

### Infrastructure

Contiene:

- Persistencia.
- Repositorios.
- EF Core/Dapper.
- Integraciones externas.
- Sistema de archivos.
- Email.
- Implementaciones técnicas.

### AI

Contiene:

- Orquestación IA.
- Prompts.
- Context builders.
- Agentes.
- Proveedores IA.
- Memoria semántica si aplica.
- Herramientas para consulta controlada.

No debe contener:

- Reglas de negocio críticas irrevisables.
- Cambios directos en producción.
- Escritura destructiva sin permisos.

### Modules

Contiene módulos funcionales:

- CRM.
- ERP.
- Documental.
- Facturación.
- Otros módulos futuros.

Cada módulo debe ser aislable, versionable y activable.

---

# 9. Sistema modular

## 9.1 Concepto

Cada módulo debe poder:

- Activarse.
- Desactivarse.
- Versionarse.
- Declarar dependencias.
- Registrar permisos.
- Registrar menús.
- Registrar entidades.
- Registrar migraciones.
- Exponer capacidades a la IA.

## 9.2 Manifiesto de módulo

Cada módulo debería tener un manifiesto similar:

```json
{
  "name": "CRM",
  "version": "1.0.0",
  "enabled": true,
  "dependencies": ["Core"],
  "permissions": [
    "crm.customers.read",
    "crm.customers.write",
    "crm.contacts.read",
    "crm.contacts.write"
  ],
  "features": [
    "customers",
    "contacts",
    "opportunities",
    "activities"
  ]
}
```

## 9.3 Tablas de sistema previstas

Propuesta inicial:

```txt
SystemTenants
SystemUsers
SystemRoles
SystemPermissions
SystemRolePermissions
SystemModules
SystemModuleVersions
SystemFeatureFlags
SystemAuditLog
SystemSettings
SystemLicenses
```

No crear todas al principio si el MVP no las necesita. Priorizar lo mínimo funcional.

---

# 10. IA supervisada

## 10.1 Rol de la IA dentro del producto

La IA debe ayudar en dos planos:

### Plano cliente

La IA debe poder:

- Explicar módulos.
- Resolver dudas funcionales.
- Guiar al usuario.
- Resumir información.
- Detectar necesidades.
- Proponer mejoras.
- Crear borradores de tareas.
- Generar documentación de procesos.
- Ayudar a configurar flujos.

### Plano técnico interno

La IA debe poder:

- Analizar requisitos.
- Revisar impacto.
- Proponer cambios.
- Generar diseño técnico.
- Preparar ramas.
- Modificar código bajo control.
- Generar migraciones.
- Generar tests.
- Crear documentación.
- Preparar notas de versión.

## 10.2 La IA no decide sola

Toda acción relevante debe tener supervisión humana.

El flujo correcto es:

```txt
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

## 10.3 Representación visual de cambios

Cuando un cliente pida una modificación, la IA debe intentar entregar:

- Descripción funcional.
- Pantallas afectadas.
- Flujo actual.
- Flujo propuesto.
- Datos afectados.
- Permisos afectados.
- Riesgos.
- Estimación de impacto.
- Plan de validación.

No debe prometer una pantalla exacta si no existe un sistema visual definido.

---

# 11. Roles humanos en la empresa proveedora

La empresa futura puede organizarse así:

## 11.1 Responsable de cliente

Persona encargada de:

- Hablar con el cliente.
- Entender necesidades.
- Traducir problemas reales a tareas.
- Validar que la IA ha entendido bien.
- Priorizar trabajo.
- Mantener relación continua.

## 11.2 Experto funcional

Persona encargada de:

- Conocer el área del cliente.
- Validar procesos.
- Revisar propuestas funcionales.
- Evitar soluciones técnicamente correctas pero empresarialmente inútiles.

## 11.3 Experto técnico

Persona encargada de:

- Revisar arquitectura.
- Validar implementación.
- Revisar migraciones.
- Revisar seguridad.
- Aprobar despliegues.

## 11.4 Supervisor IA

Persona encargada de:

- Mantener prompts.
- Revisar agentes.
- Ajustar contexto.
- Detectar errores de IA.
- Mejorar calidad de respuestas.
- Mantener documentación viva.

---

# 12. Agentes de IA propuestos

Claude Code puede usar agentes diferenciados. No crear demasiados al principio.

## 12.1 Agentes iniciales

### Product Planner Agent

Responsabilidad:

- Convertir ideas desordenadas en requisitos claros.
- Separar necesidad real de solución propuesta.
- Detectar ambigüedades.
- Preparar historias de usuario.

No debe:

- Implementar código.
- Crear migraciones.
- Decidir arquitectura técnica final.

### Solution Architect Agent

Responsabilidad:

- Diseñar arquitectura.
- Definir módulos.
- Identificar impactos.
- Detectar deuda técnica.
- Proponer estructura.

No debe:

- Implementar sin aprobación.
- Añadir complejidad innecesaria.
- Usar patrones solo por moda.

### Dotnet Backend Agent

Responsabilidad:

- Implementar casos de uso.
- Crear servicios.
- Crear endpoints.
- Mantener arquitectura limpia.
- Generar tests unitarios.

No debe:

- Tocar base de datos sin coordinar con DB Agent.
- Saltarse validaciones.
- Mezclar lógica de negocio con infraestructura.

### Database Agent

Responsabilidad:

- Diseñar tablas.
- Crear migraciones.
- Revisar índices.
- Controlar integridad.
- Evitar cambios destructivos.

No debe:

- Eliminar columnas/datos sin plan.
- Crear tablas sin justificar relaciones.
- Ignorar rendimiento.

### AI Orchestrator Agent

Responsabilidad:

- Diseñar prompts internos.
- Definir herramientas IA.
- Crear context builders.
- Diseñar memoria y RAG si aplica.
- Asegurar separación entre datos del cliente y proveedor.

No debe:

- Enviar datos sensibles a proveedores externos sin política definida.
- Dar acceso ilimitado a la IA.
- Mezclar datos entre clientes.

### QA Agent

Responsabilidad:

- Crear tests.
- Revisar casos límite.
- Validar errores.
- Probar flujos críticos.
- Detectar regresiones.

No debe:

- Considerar válido algo sin prueba.
- Reducir cobertura sin motivo.

### Code Reviewer Agent

Responsabilidad:

- Revisar diffs.
- Detectar bugs.
- Detectar incoherencias.
- Bloquear cambios peligrosos.
- Revisar seguridad.
- Revisar mantenibilidad.

No debe:

- Aprobar código incompleto.
- Dar por bueno código sin compilar.
- Ignorar riesgos por prisa.

---

# 13. Flujo obligatorio de tareas

Para cualquier tarea mediana o grande, Claude debe seguir este flujo:

## 13.1 Entender

Antes de tocar código:

1. Leer petición.
2. Identificar objetivo.
3. Identificar alcance.
4. Buscar piezas existentes.
5. Detectar riesgos.
6. Separar hechos de supuestos.

## 13.2 Planificar

Entregar un plan breve:

```md
Objetivo:
Archivos afectados:
Cambios previstos:
Riesgos:
Validación:
```

## 13.3 Implementar

Solo después de planificar.

Reglas:

- Cambios pequeños.
- No mezclar varias tareas.
- No reescribir arquitectura completa sin permiso.
- No introducir dependencias innecesarias.
- No romper compatibilidad sin avisar.

## 13.4 Validar

Siempre que sea posible:

- Compilar.
- Ejecutar tests.
- Revisar migraciones.
- Revisar diff.
- Revisar documentación.

## 13.5 Resumir

Al terminar:

```md
He cambiado:
He comprobado:
No he podido comprobar:
Riesgos pendientes:
Siguiente paso:
```

---

# 14. Flujo Git y versionado

## 14.1 Ramas

No trabajar directamente sobre `main` salvo proyecto inicial sin ramas.

Formato recomendado:

```txt
feature/module-crm-customers
feature/ai-context-builder
fix/license-validation
docs/project-foundation
```

## 14.2 Commits

Commits claros:

```txt
feat(crm): add customer entity
feat(ai): add initial provider abstraction
docs(architecture): define modular system
fix(core): validate module dependency loading
test(crm): add customer creation tests
```

## 14.3 Versionado por módulos

Cada módulo debe tener versión:

```txt
Core 1.0.0
CRM 1.0.0
Documents 1.0.0
AI 1.0.0
```

Usar SemVer cuando sea posible:

```txt
MAJOR.MINOR.PATCH
```

## 14.4 Migraciones

Toda migración debe incluir:

- Motivo.
- Impacto.
- Si es reversible.
- Riesgo.
- Datos afectados.
- Prueba mínima.

---

# 15. Seguridad y privacidad

## 15.1 Datos locales

El producto debe diseñarse para que los datos del cliente puedan quedarse en su infraestructura.

Reglas:

- No enviar datos del cliente a proveedores externos sin consentimiento.
- No mezclar datos de diferentes clientes.
- No usar datos reales para entrenar modelos sin contrato explícito.
- No registrar información sensible en logs sin anonimización.
- No exponer secretos en código.
- No incluir claves API en repositorio.

## 15.2 Secretos

Nunca guardar en el repo:

- API keys.
- Passwords.
- Connection strings reales.
- Tokens.
- Certificados privados.

Usar:

- Variables de entorno.
- Secret manager.
- Configuración local ignorada por Git.

## 15.3 Permisos

El sistema debe contemplar:

- Usuarios.
- Roles.
- Permisos.
- Módulos activos.
- Acciones auditables.
- Separación por cliente/tenant si aplica.

## 15.4 Auditoría

Acciones críticas deben poder auditarse:

- Alta de datos clave.
- Modificación de datos clave.
- Eliminaciones.
- Cambios de configuración.
- Activación/desactivación de módulos.
- Acciones IA relevantes.
- Cambios propuestos por IA.
- Aprobaciones humanas.

---

# 16. Licenciamiento y suscripción

## 16.1 Modelo comercial previsto

La plataforma debe poder venderse por:

- Cliente.
- Instalación.
- Usuarios.
- Módulos activos.
- Uso de IA.
- Soporte.
- Personalización.
- Mantenimiento.

## 16.2 Entidades futuras

Propuesta:

```txt
License
LicenseModule
LicenseFeature
SubscriptionPlan
SubscriptionInvoice
Installation
Activation
```

No implementarlas todas en el MVP salvo que sea necesario.

## 16.3 Validación de licencia

Debe permitir:

- Modo online.
- Modo offline con expiración controlada.
- Activación por instalación.
- Validación de módulos contratados.
- Bloqueo gradual, no destructivo.

No bloquear datos del cliente de forma agresiva. Si caduca la licencia, debe haber un modo seguro de consulta/exportación según contrato.

---

# 17. Módulos iniciales del MVP

## 17.1 Core

Primer módulo obligatorio.

Debe incluir:

- Usuarios.
- Roles.
- Permisos.
- Empresas/Tenants si se decide incluir desde inicio.
- Configuración.
- Auditoría básica.
- Registro de módulos.
- Menú dinámico.

## 17.2 CRM

Primer módulo funcional real.

Debe incluir:

- Clientes.
- Contactos.
- Actividades.
- Notas.
- Oportunidades simples.
- Historial.
- Búsqueda.

## 17.3 Documentos

Módulo de soporte.

Debe incluir:

- Documentos asociados a cliente.
- Versionado básico de documentos.
- Comentarios.
- Clasificación.
- Búsqueda.

## 17.4 IA

Módulo transversal.

Debe incluir:

- Chat con contexto controlado.
- Resumen de cliente.
- Explicación de módulos.
- Generación de tareas.
- Propuestas de mejora.
- Registro de conversaciones relevantes.
- Validación humana antes de cambios.

---

# 18. Fuera de alcance inicial

No implementar al principio:

- Contabilidad completa.
- Producción avanzada.
- Stock complejo.
- Facturación legal completa.
- Multi-país.
- Marketplace de módulos.
- Automatización total de código en producción.
- Entrenamiento propio de modelos.
- Microservicios complejos.
- Kubernetes.
- BI avanzado.
- Workflows visuales complejos.
- Firma digital avanzada.
- Integraciones masivas.

Estos puntos pueden existir en visión futura, pero no en MVP.

---

# 19. Documentación obligatoria

## 19.1 Estructura recomendada

```txt
docs/
├── product/
│   ├── vision.md
│   ├── business-model.md
│   └── roadmap.md
├── architecture/
│   ├── overview.md
│   ├── modular-system.md
│   ├── ai-supervision.md
│   ├── security.md
│   └── deployment.md
├── decisions/
│   ├── ADR-0001-project-scope.md
│   ├── ADR-0002-initial-stack.md
│   └── ADR-0003-ai-governance.md
├── modules/
│   ├── core.md
│   ├── crm.md
│   ├── documents.md
│   └── ai.md
└── operations/
    ├── git-flow.md
    ├── release-process.md
    └── testing.md
```

## 19.2 ADR

Toda decisión arquitectónica relevante debe ir en `/docs/decisions`.

Formato:

```md
# ADR-000X - Título

## Estado
Propuesta | Aceptada | Rechazada | Sustituida

## Contexto

## Decisión

## Consecuencias

## Alternativas consideradas
```

## 19.3 Documentar antes de escalar

Si una idea no puede explicarse en documentación simple, probablemente aún no está madura para implementarse.

---

# 20. Reglas de código

## 20.1 General

- Código claro antes que código ingenioso.
- Nombres explícitos.
- Evitar clases enormes.
- Evitar servicios con demasiadas responsabilidades.
- No duplicar lógica crítica.
- No introducir abstracciones sin necesidad.
- No mezclar capas.
- No usar magia que dificulte depuración.

## 20.2 C#

Reglas preferentes:

- Usar nullable reference types si el proyecto lo permite.
- Usar async/await correctamente.
- Evitar `async void`.
- Validar entradas.
- Preferir inyección de dependencias.
- Evitar lógica de negocio en controllers.
- Evitar modelos anémicos si las reglas pertenecen al dominio.
- No capturar excepciones para ocultarlas.

## 20.3 APIs

Controllers mínimos.

La lógica debe ir en Application.

Ejemplo conceptual:

```txt
Controller → Command/Query → Handler/Service → Domain/Repository
```

## 20.4 Errores

Los errores deben ser claros y trazables.

No devolver errores genéricos si hay causa funcional clara.

No exponer detalles internos sensibles al usuario final.

## 20.5 Logs

Los logs deben incluir contexto útil:

- Acción.
- Usuario si aplica.
- Tenant si aplica.
- Entidad afectada.
- Identificador.
- Resultado.
- Error.

No incluir datos sensibles salvo justificación y protección.

---

# 21. Base de datos

## 21.1 Reglas

- No modificar estructura sin migración.
- No borrar columnas sin plan de compatibilidad.
- No usar nombres ambiguos.
- Crear índices cuando haya filtros frecuentes.
- Mantener claves primarias claras.
- Mantener claves foráneas donde proceda.
- Auditar cambios críticos.
- Preparar el diseño para multiempresa si se decide.

## 21.2 Convenciones sugeridas

Tablas en singular o plural, pero mantener una sola convención en todo el proyecto.

Campos comunes posibles:

```txt
Id
CreatedAt
CreatedBy
UpdatedAt
UpdatedBy
DeletedAt
DeletedBy
IsDeleted
TenantId
```

No añadir todos automáticamente a todas las tablas si no procede.

---

# 22. Testing

## 22.1 Reglas

Todo cambio relevante debe tener prueba.

Tipos:

- Unit tests.
- Integration tests.
- API tests.
- AI prompt tests si aplica.
- Migration tests si aplica.

## 22.2 No afirmar validez sin pruebas

Si no se han ejecutado tests, decirlo.

Formato:

```md
Validación:
- Compilación: no ejecutada.
- Tests: no ejecutados.
- Revisión manual: realizada sobre archivos X.
```

o

```md
Validación:
- `dotnet build`: correcto.
- `dotnet test`: correcto.
```

## 22.3 Casos críticos

Probar especialmente:

- Permisos.
- Multiempresa.
- Activación/desactivación de módulos.
- Migraciones.
- Acceso a datos por IA.
- Licencias.
- Auditoría.
- Errores funcionales.

---

# 23. IA dentro del producto

## 23.1 Contexto IA

La IA no debe recibir todo por defecto.

Debe recibir contexto controlado:

- Usuario actual.
- Empresa/Tenant.
- Módulos activos.
- Permisos.
- Datos relevantes.
- Historial limitado.
- Documentación aplicable.
- Objetivo de la tarea.

## 23.2 Memoria

La memoria IA debe ser explícita.

Tipos:

```txt
Memoria funcional
Memoria técnica
Memoria de cliente
Memoria de módulo
Memoria de decisiones
Memoria temporal de conversación
```

No mezclar memorias entre clientes.

## 23.3 RAG

Si se implementa búsqueda semántica, debe:

- Respetar permisos.
- Filtrar por cliente.
- Citar o referenciar documentos internos.
- No responder con documentos no accesibles.
- Indicar incertidumbre si el contexto es insuficiente.

## 23.4 Herramientas IA

Las herramientas de IA deben tener permisos.

Ejemplos:

```txt
ReadCustomer
SearchDocuments
CreateDraftTask
GenerateChangeProposal
ReadModuleDocumentation
CreateTechnicalPlan
CreateBranch
GenerateCodePatch
RunTests
```

Herramientas peligrosas deben requerir aprobación:

```txt
ApplyMigration
WriteProductionData
DeployVersion
DeleteData
ChangeLicense
```

---

# 24. Flujo de cambio solicitado por cliente

Cuando un cliente pide una mejora, seguir este flujo:

```txt
1. Registrar petición.
2. Clasificar: duda, incidencia, mejora, nuevo módulo, configuración.
3. IA genera resumen.
4. Responsable humano valida entendimiento.
5. IA propone solución funcional.
6. Experto funcional aprueba o corrige.
7. IA propone diseño técnico.
8. Experto técnico aprueba o corrige.
9. IA prepara implementación en rama.
10. Tests.
11. Revisión.
12. Demo o representación.
13. Aprobación cliente si aplica.
14. Despliegue versionado.
15. Documentación y notas de versión.
```

---

# 25. Representación de propuestas al cliente

Toda propuesta generada por IA para un cliente debe incluir:

```md
# Propuesta de cambio

## Necesidad detectada

## Situación actual

## Solución propuesta

## Pantallas o módulos afectados

## Flujo propuesto

## Datos afectados

## Permisos afectados

## Riesgos

## Alternativas

## Estimación de complejidad
Baja | Media | Alta

## Requiere desarrollo
Sí | No

## Requiere configuración
Sí | No

## Requiere validación humana
Sí
```

---

# 26. Niveles de automatización IA

## Nivel 0 - Consulta

La IA responde dudas.

## Nivel 1 - Documentación

La IA genera documentación, resúmenes y propuestas.

## Nivel 2 - Configuración asistida

La IA propone cambios de configuración, pero no los aplica sin confirmación.

## Nivel 3 - Código asistido

La IA genera ramas, código, migraciones y tests.

## Nivel 4 - Integración supervisada

La IA prepara pull requests completos.

## Nivel 5 - Automatización avanzada

Solo en entornos controlados. No producción directa.

El proyecto debe empezar en Nivel 1-2, no en Nivel 5.

---

# 27. Definición de terminado

Una tarea se considera terminada solo si:

- El objetivo está claro.
- El cambio está implementado.
- El código compila o se indica que no se pudo comprobar.
- Los tests relevantes pasan o se indica que no se ejecutaron.
- La documentación se actualiza si aplica.
- El riesgo queda explicado.
- El diff se puede revisar.
- No hay cambios ocultos.
- No hay supuestos sin marcar.

---

# 28. Proceso recomendado con Claude Code

## 28.1 Primera sesión

Pasos recomendados:

```txt
/init
/memory
/permissions
/agents
/mcp
```

Usar `/init` solo para generar una base inicial si no existe este archivo. Si este archivo ya existe, conservarlo y refinarlo.

## 28.2 Cambios grandes

Antes de cambios grandes:

```txt
/plan
```

Después:

```txt
/diff
/code-review
```

Si se trabaja con ramas o worktrees, mantener cada tarea aislada.

## 28.3 Permisos

Claude debe pedir aprobación para:

- Crear o borrar muchos archivos.
- Ejecutar comandos destructivos.
- Instalar paquetes.
- Modificar migraciones.
- Tocar configuración de despliegue.
- Cambiar permisos.
- Acceder a secretos.
- Ejecutar scripts sobre datos reales.

---

# 29. MCP

## 29.1 Uso previsto

MCP puede conectar la IA con:

- Repositorio.
- Documentación.
- Base de datos local.
- Sistema de tickets.
- Logs.
- Archivos.
- Herramientas de despliegue.
- Entornos de pruebas.

## 29.2 Reglas

No conectar MCP a datos reales sin permisos claros.

Toda herramienta MCP debe tener:

- Nombre claro.
- Alcance.
- Permisos.
- Entorno permitido.
- Riesgos.
- Logs.

Ejemplo:

```txt
mcp.db.readonly.dev
mcp.docs.write
mcp.git.branch
mcp.tests.run
mcp.deploy.staging
```

---

# 30. Roadmap inicial

## Fase 0 - Cimientos

Objetivo:

- Crear repositorio.
- Crear documentación base.
- Crear `CLAUDE.md`.
- Definir visión.
- Definir arquitectura inicial.
- Definir flujo Git.
- Definir agentes.

Resultado:

- Proyecto preparado para desarrollo asistido por IA.

## Fase 1 - Solución base

Objetivo:

- Crear solución .NET.
- Crear capas.
- Crear Core mínimo.
- Crear configuración.
- Crear tests iniciales.

Resultado:

- Base técnica compilable.

## Fase 2 - Módulo Core

Objetivo:

- Usuarios.
- Roles.
- Permisos.
- Módulos.
- Auditoría básica.

Resultado:

- Plataforma con estructura empresarial mínima.

## Fase 3 - Módulo CRM

Objetivo:

- Clientes.
- Contactos.
- Notas.
- Actividades.
- Búsqueda.

Resultado:

- Primer módulo funcional real.

## Fase 4 - IA documental

Objetivo:

- Chat IA con contexto del CRM.
- Resumen de cliente.
- Consulta de documentación.
- Generación de propuestas.

Resultado:

- IA útil sin tocar código.

## Fase 5 - IA técnica supervisada

Objetivo:

- Generación de planes técnicos.
- Generación de ramas.
- Generación de código.
- Generación de tests.
- Pull requests supervisados.

Resultado:

- IA como copiloto técnico controlado.

## Fase 6 - Licenciamiento

Objetivo:

- Planes.
- Módulos contratados.
- Activación.
- Validación básica.

Resultado:

- Base para modelo comercial.

## Fase 7 - Despliegue local

Objetivo:

- Docker Compose o instalador.
- Configuración por cliente.
- Backups.
- Actualizaciones.

Resultado:

- Instalación realista en empresa cliente.

---

# 31. Preguntas que Claude debe hacerse antes de implementar

Antes de tocar código, responder internamente:

1. ¿Existe ya una pieza parecida?
2. ¿Estoy duplicando lógica?
3. ¿La tarea pertenece al MVP?
4. ¿Estoy mezclando capas?
5. ¿El cambio afecta seguridad?
6. ¿El cambio afecta datos?
7. ¿El cambio necesita migración?
8. ¿El cambio necesita tests?
9. ¿El cambio necesita documentación?
10. ¿Estoy asumiendo algo no validado?

Si alguna respuesta implica riesgo, comunicarlo.

---

# 32. Respuesta estándar ante tareas ambiguas

Cuando una petición sea ambigua, responder así:

```md
Entiendo que quieres [interpretación].

Antes de implementar, veo estas ambigüedades:
1. [Ambigüedad]
2. [Ambigüedad]

Para avanzar sin bloquear, asumiré:
- [Supuesto seguro]

Plan:
1. [Paso]
2. [Paso]
3. [Paso]
```

Si la ambigüedad puede romper arquitectura o datos, no implementar sin aclaración.

---

# 33. Respuesta estándar para análisis de código

```md
Objetivo del análisis:
[Objetivo]

Archivos revisados:
- [Archivo]

Hallazgos:
1. [Hallazgo]
   - Impacto:
   - Riesgo:
   - Propuesta:

Cambios recomendados:
1. [Cambio]

Validación necesaria:
- [Test/compilación/revisión]
```

---

# 34. Respuesta estándar para implementación

```md
Objetivo:
[Objetivo]

Cambios realizados:
- [Cambio 1]
- [Cambio 2]

Archivos modificados:
- [Archivo 1]
- [Archivo 2]

Validación:
- Build: [resultado]
- Tests: [resultado]
- Revisión manual: [resultado]

Riesgos:
- [Riesgo]

Siguiente paso:
- [Paso]
```

---

# 35. Nombres provisionales del producto

No fijar nombre definitivo sin decisión humana.

Opciones iniciales:

- ModularAI ERP.
- CorePilot ERP.
- NeuralCore CRM.
- AtlasCore AI.
- EmpresaOS AI.
- NexusERP AI.
- Debales Core AI.

Si se usa un nombre en código, dejar claro que es provisional.

---

# 36. Filosofía de producto

No competir como “otro ERP genérico”.

Competir como:

> Plataforma empresarial modular que aprende el funcionamiento de cada empresa y ayuda a adaptar sus procesos mediante IA supervisada.

La IA no debe venderse como sustituto del equipo técnico.

Debe venderse como multiplicador del equipo técnico.

---

# 37. Riesgos principales

## 37.1 Riesgo de sobrealcance

Intentar crear CRM, ERP, IA, licencias, despliegue, BI y automatización completa desde el día uno.

Mitigación:

- MVP pequeño.
- Fases claras.
- Módulos mínimos.
- Documentación de alcance.

## 37.2 Riesgo de IA peligrosa

Permitir que la IA modifique producción o datos reales sin control.

Mitigación:

- Supervisión humana.
- Permisos.
- Entornos separados.
- Logs.
- Revisión.
- Tests.

## 37.3 Riesgo de arquitectura inflada

Meter microservicios, colas, Kubernetes y patrones complejos demasiado pronto.

Mitigación:

- Monolito modular inicial.
- Separación interna clara.
- Escalar solo cuando duela.

## 37.4 Riesgo legal y privacidad

Enviar datos de cliente a IA externa sin consentimiento.

Mitigación:

- Proveedores configurables.
- IA local opcional.
- Anonimización.
- Contratos.
- Trazabilidad.

## 37.5 Riesgo comercial

Construir demasiado antes de validar necesidad real.

Mitigación:

- Primer módulo vendible.
- Demostración temprana.
- Feedback real.
- Ciclos cortos.

---

# 38. Decisiones iniciales recomendadas

## 38.1 Arquitectura

Usar monolito modular.

Motivo:

- Más simple.
- Más rápido.
- Más mantenible para un proyecto personal inicial.
- Permite escalar más adelante.

## 38.2 Primer módulo

Empezar por CRM.

Motivo:

- Menor complejidad que ERP.
- Más demostrable.
- Útil para cualquier empresa.
- Buen terreno para IA documental.

## 38.3 IA inicial

Empezar con IA documental y funcional.

No empezar con IA autoprogramadora completa.

Motivo:

- Menor riesgo.
- Más valor inmediato.
- Mejor validación de producto.

## 38.4 Datos

Diseñar pensando en local/on-premise.

Motivo:

- Diferencial comercial.
- Confianza empresarial.
- Control de datos.

---

# 39. Comandos útiles previstos

No ejecutar sin contexto. Usar según necesidad.

```bash
dotnet new sln
dotnet new webapi
dotnet new classlib
dotnet new xunit
dotnet build
dotnet test
dotnet ef migrations add
dotnet ef database update
git status
git diff
git checkout -b feature/name
```

Antes de ejecutar comandos destructivos, pedir confirmación.

Comandos destructivos incluyen:

```bash
rm -rf
git reset --hard
git clean -fd
drop database
dotnet ef database drop
docker volume rm
```

---

# 40. Formato de trabajo preferido para este proyecto

Cuando Carlos pida avanzar en el proyecto, estructurar la respuesta así:

```md
## Objetivo del paso

## Archivos afectados

## Implementación

## Prueba

## Resultado esperado

## Riesgos

## Siguiente paso
```

Mantener el avance por fases. No saltar de fase sin motivo.

---

# 41. Regla final

Si una decisión no está clara, Claude debe preferir:

1. Seguridad.
2. Claridad.
3. Simplicidad.
4. Trazabilidad.
5. MVP.
6. Escalabilidad progresiva.

No priorizar velocidad por encima de fiabilidad.

El proyecto debe crecer como producto real, no como demo caótica.
