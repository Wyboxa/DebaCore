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

# 6. Estado actual del proyecto

> **Actualización 2026-06-14 (2):** Importación masiva CSV — `CsvParser` + `ImportResult` en Application.Common. Handlers: `ImportCustomersHandler`, `ImportSuppliersHandler`, `ImportItemsHandler`. `ImportController` (POST /api/import/*). UI `/configuracion/importacion`. `ImportResultCard` componente compartido. 8 tests nuevos (CsvParserTests + ImportCustomersHandlerTests). Multi-tenant estructural: `TenantId` en `AuditableEntity`, `ITenantService`, `HttpContextTenantService`, `SetTenantIds()` en DbContext. Migración `20260614160000_AddTenantIdToBusinessEntities`. Subida real de archivos: `IFileStorageService` / `LocalFileStorageService`, `UploadDocumentFileHandler`, endpoint `POST /api/documents/{id}/upload`. Integración Chat IA → propuesta: botón "Guardar como propuesta IA" en `IA.razor`.
> **Actualización 2026-06-14:** Módulo `AIGovernance` completo 7 capas — Domain (`AIRule`, `AIKnowledgeBase`, `AIActionProposal`, `AIActionApproval`, `AIExecutionLog`) + Application (13 handlers) + Infrastructure (5 repos + 5 EF configs) + API (`AIGovernanceController`) + Web (`Propuestas.razor`, `PropuestaDetalle.razor`, `AIReglas.razor`, `AIConocimiento.razor`) + NavMenu. Tab "Documentos" añadido a `CustomerDetail` y `SupplierDetail` (lazy). Migración manual `20260614090000_AddAIGovernanceModule`.
> **Actualización 2026-06-13:** Módulo `Documents` completo 7 capas — Domain (`Document`, `DocumentType`) + Application (8 handlers) + Infrastructure + API (`DocumentsController`, `DocumentTypesController`) + Web (`Documentos.razor`, `DocumentoDetalle.razor`, `TiposDocumento.razor`) + NavMenu. Migración manual `20260613120000_AddDocumentsModule` (regenerar con `dotnet ef migrations add` tras instalar .NET).
> **Actualización 2026-06-12:** Tutorial mejorado a 12 pasos con Tips. Analítica ampliada (evolución mensual, top proveedores, conversión presupuestos). `SupplierContact` — módulo completo 7 capas: Domain + Application (4 handlers) + Infrastructure + API + Web (tab "Contactos" en ficha proveedor). Migración `20260612141224_AddSupplierContactModule`.
> 26 migraciones aplicadas (incluye 3 manuales + TenantId). 231 tests (223 previos + 8 nuevos de CsvParser/ImportCustomers). Vault Obsidian activo en `docs/obsidian/` — se actualiza en cada sesión.
> Las secciones siguientes describen la arquitectura objetivo completa del producto, no solo lo ya implementado.

El proyecto cuenta con la plataforma ERP completa y la capa de IA supervisada sobre ERP.

Lo que ya existe (no asumir como propuesta):

- Solución .NET 8 (`Debales.slnx`) con 10 proyectos.
- Base de datos SQL Server LocalDB con **13 migraciones aplicadas**.
- Módulo Core: usuarios, roles, permisos, auditoría. **UI de gestión de usuarios** (`/configuracion/usuarios`).
- Módulo CRM: clientes, contactos, actividades, notas, oportunidades.
- Módulo Suppliers: proveedores con búsqueda, paginación, dirección embebida, soft-delete.
- Módulo Catalog: artículos, familias, unidades de medida, tipos de IVA.
- Módulo Sales: presupuestos (`AddSalesQuoteModule`), pedidos, albaranes, facturas, rectificativas, vencimientos, cobros. Flujo completo: **Presupuesto → Pedido → Albarán → Factura**. `AddERP2Module` + `AddERP3Module` + `AddSalesQuoteModule`.
- Módulo Purchasing: pedidos, albaranes (con actualización de estado de pedido), facturas, rectificativas, vencimientos, pagos. Flujo espejo implementado. `AddERP2Module` + `AddERP3Module`.
- Módulo Inventory: almacenes, ubicaciones, movimientos de stock (generados automáticamente desde albaranes), saldos. `AddERP4Module`.
- Módulo Accounting: plan contable, cuentas, diarios, ejercicios, períodos, asientos. **Informes**: balance de comprobación, libro diario, balance de situación. `AddAccountingModule`.
- Módulo AI (ERP-6): chat financiero con contexto ERP, detección de anomalías, resúmenes cliente/proveedor, aprobación humana.
- Módulo Licensing: entidades `License`, `LicenseModule`, `SubscriptionPlan`, controller, UI. `ModuleRequired` (guard en hubs de UI). `AddLicensingModule`.
- Despliegue Docker: `docker-compose.yml`, `Dockerfile.api`, `Dockerfile.web` en raíz del repositorio.
- PDF export de facturas venta y compra (QuestPDF Community). Endpoints en `Debales.Web/Program.cs`.
- API REST (`Debales.Api`) con controllers para todos los módulos.
- UI Blazor Server (`Debales.Web`) con lista y ficha de clientes, proveedores, ventas, presupuestos, compras, inventario, contabilidad, informes, licencias, gestión de usuarios e IA. **Tutorial guiado** activable/desactivable desde Configuración (`TutorialService`, `TutorialOverlay`, 8 pasos, localStorage). **AuditLog UI** (`/configuracion/auditoria`). **Dashboard KPIs** (`Home.razor`, 6 KPIs con importes € + alertas cobros/pagos vencidos + actividad reciente). **Series documentales** (`/configuracion/series`).
- `AccountCode` en `Customer` y `Supplier`: editable desde ficha, propagado en handlers y DTOs. `AccountingEntryService` usa fallback 430/400 cuando la cuenta individual es null.
- Sistema de auditoría automática: `ApplicationDbContext.SaveChangesAsync` escribe `AuditEntry` para todas las entidades del dominio (excepto líneas y tablas de control). `ICurrentUserService` / `CurrentUserService` para captura del usuario activo.
- Módulo NumberSeries: `NumberSeries.Consume()` genera y consume numeración; `GetNextFormatted()` previsualiza. UI en `/configuracion/series`. **Cableado completo en los 13 handlers de creación de documentos** (PRE, PV, ALV, FV, RV, PC, ALC, FC, RC). Migración seed `AddNumberSeriesSeed` con 9 series por defecto (pendiente de aplicar con `database update`).
- UI con paleta teal `#6B9CA9`, sidebar oscuro.
- **72 tests automatizados pasando** (Domain: 31, Application: 40, Integration: 1).
- Vault Obsidian en `docs/obsidian/` — actualizado en cada sesión de desarrollo.

Lo que aún no existe y debe tratarse como propuesta:

- `ModuleRequired` en páginas de lista/detalle individuales (solo en hubs por ahora).
- Multi-tenant (`TenantId` en tablas).

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

> **Nota 2026-05-27:** El proyecto real usa el prefijo `Debales.*` (nombre definitivo decidido por Carlos).

Estructura real del proyecto:

```txt
src/
├── Debales.Api/           ← API REST (implementada)
├── Debales.Web/           ← Blazor Server UI (implementada)
├── Debales.Application/   ← Casos de uso, handlers, DTOs (implementada)
├── Debales.Domain/        ← Entidades, value objects, reglas (implementada)
├── Debales.Infrastructure/ ← EF Core, repositorios, persistencia (implementada)
└── Debales.AI/            ← Orquestación IA (estructura vacía, fase futura)

tests/
├── Debales.Domain.Tests/
├── Debales.Application.Tests/
└── Debales.Integration.Tests/
```

Estructura conceptual objetivo (módulos futuros):

```txt
src/
└── Debales.Modules/
    ├── Core/
    ├── CRM/
    ├── Sales/
    ├── Purchasing/
    ├── Catalog/
    ├── Inventory/
    ├── Accounting/
    ├── Documents/
    └── AI/
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

- Contabilidad completa (sí se prevé contabilidad mínima como base — ver §43 y §46).
- Producción avanzada.
- Stock complejo (sí se prevé stock básico en fases ERP).
- Facturación legal completa (sí se prevén fundamentos de facturación — ver §46).
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

No implementar como núcleo en ninguna fase:

- Escandallos industriales específicos.
- Mamparas o cálculo de mamparas.
- Órdenes de fabricación.
- Partes de fabricación o consumo.
- Procesos industriales específicos de clientes concretos.
- Plantillas específicas de clientes.
- Características de instalaciones concretas.

Estos elementos verticales pueden existir más adelante como módulos opcionales sobre el núcleo, pero no contaminan la base del producto.

Estos puntos pueden existir en visión futura, pero no en el núcleo del MVP.

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

# 35. Nombre del producto

> **Decisión 2026-05-27:** El nombre del producto es **Debales**.
> Decidido por Carlos. Todos los proyectos, namespaces y artefactos usan el prefijo `Debales.*`.

El nombre no es provisional. Usar "Debales" en código, documentación y comunicación del proyecto.

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

---

# 42. Fundamentos del CRM/ERP modular

Esta sección documenta el catálogo de entidades objetivo del producto completo.
No implica que deban implementarse todas de inmediato. Es la referencia arquitectónica de dirección.

## 42.1 Core

```txt
Tenant / Company
User
Role
Permission
Module
Setting
NumberSeries          ← series documentales (FAC, PED, ALB…)
AuditLog
DocumentAttachment
SystemParameter
```

## 42.2 CRM

```txt
Customer
CustomerContact
CustomerAddress
CustomerActivity
CustomerNote
CustomerOpportunity
```

## 42.3 Proveedores

```txt
Supplier
SupplierContact
SupplierAddress
```

## 42.4 Catálogo

```txt
Item                  ← artículo
Service               ← servicio
ItemFamily
UnitOfMeasure
TaxType               ← tipo de IVA
PriceList             ← tarifa
ItemPrice
SupplierItemCode      ← código del proveedor para el artículo
CustomerItemCode      ← código del cliente para el artículo
```

## 42.5 Ventas

```txt
SalesQuote            ← presupuesto de venta
SalesQuoteLine
SalesOrder            ← pedido cliente
SalesOrderLine
SalesDeliveryNote     ← albarán de venta
SalesDeliveryNoteLine
SalesInvoice          ← factura de venta
SalesInvoiceLine
SalesCreditNote       ← factura rectificativa de venta
Receivable            ← vencimiento de cobro
CustomerPayment       ← cobro
```

## 42.6 Compras

```txt
PurchaseOrder         ← pedido proveedor
PurchaseOrderLine
PurchaseDeliveryNote  ← albarán de compra
PurchaseDeliveryNoteLine
PurchaseInvoice       ← factura de compra
PurchaseInvoiceLine
PurchaseCreditNote    ← factura rectificativa de compra
Payable               ← vencimiento de pago
SupplierPayment       ← pago
```

## 42.7 Almacén

```txt
Warehouse
WarehouseLocation
StockMovement
StockBalance
StockAdjustment
InventoryCount
```

## 42.8 Facturación

```txt
InvoiceSeries         ← serie de facturación
PaymentTerm           ← condición de pago
PaymentMethod         ← forma de pago
```

## 42.9 Contabilidad

```txt
ChartOfAccounts       ← plan contable
Account               ← cuenta contable
AccountingJournal     ← diario contable
FiscalYear            ← ejercicio contable
FiscalPeriod          ← periodo contable
AccountingEntry       ← asiento contable
AccountingEntryLine   ← línea de asiento
BankAccount
CashAccount
Remittance            ← remesa
```

## 42.10 Documentos

```txt
Document
DocumentAttachment
DocumentTemplate
DocumentVersion
DocumentType
```

## 42.11 Auditoría

```txt
AuditLog
EntityChange
```

## 42.12 IA supervisada

```txt
AIContext
AIKnowledgeBase
AIRule
AIActionProposal
AIActionApproval
AIExecutionLog
```

---

# 43. Fundamentos contables

La contabilidad nace de eventos operativos confirmados. No de pantallas ni de acciones directas del usuario.

## 43.1 Principio de separación

```txt
Documento operativo != Documento contable
```

Ejemplos de documentos **operativos**:

```txt
SalesOrder, SalesDeliveryNote, SalesInvoice
PurchaseOrder, PurchaseDeliveryNote, PurchaseInvoice
StockMovement
```

Ejemplos de documentos **contables**:

```txt
AccountingEntry, AccountingEntryLine
Receivable, CustomerPayment
Payable, SupplierPayment
Remittance
```

La factura puede generar asientos y vencimientos, pero no es ella misma el asiento.

## 43.2 Flujo de ventas

```txt
Customer
 └── SalesQuote            (no genera contabilidad)
      └── SalesOrder        (no genera contabilidad)
           └── SalesDeliveryNote  (no genera contabilidad normalmente)
                └── SalesInvoice
                     ├── AccountingEntry   ← asiento de factura
                     └── Receivable        ← vencimiento de cobro
                          └── CustomerPayment
                               └── AccountingEntry  ← asiento de cobro
```

## 43.3 Flujo de compras

```txt
Supplier
 └── PurchaseOrder         (no genera contabilidad)
      └── PurchaseDeliveryNote  (puede afectar stock)
           └── PurchaseInvoice
                ├── AccountingEntry   ← asiento de factura
                └── Payable           ← vencimiento de pago
                     └── SupplierPayment
                          └── AccountingEntry  ← asiento de pago
```

## 43.4 Eventos contables base

Los asientos se generan desde eventos de negocio confirmados:

```txt
SalesInvoicePosted
SalesInvoiceCancelled
PurchaseInvoicePosted
PurchaseInvoiceCancelled
CustomerPaymentConfirmed
SupplierPaymentConfirmed
SalesCreditNotePosted
PurchaseCreditNotePosted
StockAdjustmentConfirmed
PaymentDifferenceDeclared
CustomerDefaultRegistered
RemittanceGenerated
RemittanceConfirmed
FiscalYearClosed
FiscalYearOpened
```

## 43.5 Regla básica del asiento

```txt
TotalDebe == TotalHaber
```

Ningún asiento validado puede quedar descuadrado. Este invariante debe estar garantizado en el dominio.

## 43.6 Estados del asiento

```txt
Draft     ← borrador, modificable
Posted    ← validado, no modificable
Cancelled ← anulado, con trazabilidad
Locked    ← bloqueado por cierre de periodo
```

## 43.7 Estados del vencimiento

```txt
Pending   ← pendiente de cobro/pago
Partial   ← cobrado/pagado parcialmente
Settled   ← cobrado/pagado completamente
Defaulted ← impagado
Cancelled ← cancelado
```

## 43.8 Reglas de contabilidad (no negociables)

- No contabilizar en ejercicio cerrado.
- No contabilizar en periodo cerrado.
- No crear asientos con Debe ≠ Haber.
- No crear líneas con Debe y Haber simultáneamente.
- No imputar en cuentas no imputables.
- No modificar asientos en estado `Posted` o `Locked`.
- Toda anulación genera trazabilidad de la razón.
- Toda factura contabilizada mantiene referencia a su asiento.
- Todo cobro/pago se relaciona con uno o varios vencimientos.
- No borrar documentos contables validados: anular o rectificar.

---

# 44. Modelo conceptual de referencia

Referencia de alto nivel del modelo de datos objetivo. No implica implementación inmediata.

```txt
Core
├── Tenant / Company
├── User, Role, Permission
├── Module, Setting
├── NumberSeries, AuditLog

CRM
├── Customer ──> CustomerContact, CustomerAddress
│               CustomerActivity, CustomerNote, CustomerOpportunity

Suppliers
├── Supplier ──> SupplierContact, SupplierAddress

Catalog
├── Item, Service, ItemFamily
├── UnitOfMeasure, TaxType
├── PriceList ──> ItemPrice
└── SupplierItemCode, CustomerItemCode

Sales
├── SalesQuote ──> SalesQuoteLine
├── SalesOrder ──> SalesOrderLine
├── SalesDeliveryNote ──> SalesDeliveryNoteLine
├── SalesInvoice ──> SalesInvoiceLine
├── SalesCreditNote
├── Receivable
└── CustomerPayment

Purchasing
├── PurchaseOrder ──> PurchaseOrderLine
├── PurchaseDeliveryNote ──> PurchaseDeliveryNoteLine
├── PurchaseInvoice ──> PurchaseInvoiceLine
├── PurchaseCreditNote
├── Payable
└── SupplierPayment

Inventory
├── Warehouse ──> WarehouseLocation
├── StockMovement
├── StockBalance
└── StockAdjustment, InventoryCount

Accounting
├── ChartOfAccounts ──> Account
├── AccountingJournal
├── FiscalYear ──> FiscalPeriod
├── AccountingEntry ──> AccountingEntryLine
├── BankAccount, CashAccount
└── Remittance

Documents
├── Document ──> DocumentAttachment, DocumentVersion
└── DocumentTemplate, DocumentType

AI
├── AIContext, AIKnowledgeBase, AIRule
├── AIActionProposal ──> AIActionApproval
└── AIExecutionLog
```

---

# 45. Exclusiones — Módulos verticales

Esta sección es explícita para proteger el núcleo del producto.

## 45.1 Qué NO forma parte del núcleo

Los siguientes elementos no deben implementarse en el núcleo del producto, independientemente del cliente que los necesite:

- Cálculo de mamparas o estructuras similares.
- Escandallos industriales específicos.
- Órdenes de fabricación complejas.
- Partes de trabajo específicos de sector.
- Partes de consumo de materiales industriales.
- Plantillas de documentos de clientes concretos.
- Procesos industriales verticales.
- Configuradores de producto complejos.
- Integraciones específicas de clientes existentes.

## 45.2 Cómo implementar necesidades verticales

Si un cliente necesita funcionalidad vertical:

1. Se documenta como módulo opcional sobre el núcleo.
2. Tiene su propio namespace, versionado y manifiesto.
3. No modifica las entidades core.
4. Puede extender entidades core via composición o referencias.
5. Se activa/desactiva sin afectar el núcleo.

Ejemplo correcto:

```txt
Debales.Modules.Manufacturing.Windows  ← módulo vertical de mamparas
  └── depende de: Core, Catalog, Sales
  └── no modifica: SalesOrder, Item, Customer
  └── extiende: vía WindowsOrder, WindowsCalculation
```

---

# 46. Roadmap ERP ampliado

> **Actualización 2026-06-05:** ERP-1 a ERP-6, Fase 6 (Licenciamiento), Fase 7 (Docker), ERP-Quote (Presupuestos), ERP-Reports (Informes contables) y paridad Compras/Ventas completadas.
> El roadmap original (§30) describe la progresión técnica base. Este roadmap describe la expansión funcional ERP.

## Fase ERP-1 — Proveedores y catálogo base ✓ COMPLETA

Commit: `feat(erp1-catalog)` | Migraciones: `AddSuppliersModule`, `AddCatalogModule`.

## Fase ERP-2 — Ventas y compras básicas ✓ COMPLETA

Commit: `feat(erp2)` | Migración: `AddERP2Module`.

## Fase ERP-3 — Facturación ✓ COMPLETA

Commit: `feat(erp3)` | Migración: `AddERP3Module`.

## Fase ERP-4 — Almacén básico ✓ COMPLETA

Commit: `feat(erp4)` | Migración: `AddERP4Module`.

## Fase ERP-5 — Contabilidad mínima ✓ COMPLETA

Commit: `feat(erp5)` | Migración: `AddAccountingModule`.

## Fase ERP-6 — IA supervisada sobre ERP ✓ COMPLETA

Commit: `feat(erp6)` | Sin migración propia (usa datos de módulos anteriores).

## Fase 6 — Licenciamiento ✓ COMPLETA

Commit: `feat(licensing)` | Migración: `20260604121322_AddLicensingModule`.

Implementado: entidades `License`, `LicenseModule`, `SubscriptionPlan`, controller, UI, handlers, repositorio. Middleware completo: `RequiresModuleAttribute` (API, 15 controllers) + `ModuleRouteGuard` en `MainLayout` (Web, 10 rutas protegidas).

## Fase 7 — Despliegue Docker ✓ COMPLETA

Archivos: `docker-compose.yml`, `Dockerfile.api`, `Dockerfile.web` en raíz del repositorio.

## Fase ERP-Quote — Presupuestos de venta ✓ COMPLETA

Commit: `feat(erp-quote)` | Migración: `20260604230558_AddSalesQuoteModule`.

Implementado: `SalesQuote`, `SalesQuoteLine`, estados Draft→Sent→Accepted→Convertido. Handler `ConvertQuoteToOrderHandler`. UI: `/ventas/presupuestos` + detalle. Ciclo completo: Presupuesto → Pedido → Albarán → Factura.

## Fase ERP-Reports — Informes contables ✓ COMPLETA

Commit: `feat(accounting)` | Sin migración propia (lee de datos existentes).

Implementado: `GetTrialBalanceHandler`, `GetJournalBookHandler`, `GetBalanceSheetHandler`. UI: `/contabilidad/informes` con 3 tabs (balance de comprobación, libro diario, balance de situación).

## Correcciones de paridad Compras/Ventas ✓ COMPLETA

Commit: `fix(purchasing)`.

`PostPurchaseDeliveryNoteHandler` ahora actualiza `PurchaseOrder.Status` al confirmar albarán (igual que ventas). `PurchaseOrderLine.RecordReceipt` y `PurchaseOrder.UpdateReceiptStatus` son `public`.

---

# 47. Decisiones pendientes

Esta sección registra conflictos detectados y decisiones aún no tomadas.

## 47.1 Conflictos resueltos

| Conflicto | Resolución | Fecha |
|---|---|---|
| §6 decía que nada existe | Actualizado con estado real (Fases 0-3 completadas) | 2026-06-04 |
| §8.1 usaba `ModularAIERP.*` | Actualizado para reflejar nombre real `Debales.*` | 2026-06-04 |
| §18 ponía contabilidad como fuera de alcance | Clarificado: "completa" sigue fuera, "mínima" está en Fase ERP-5 | 2026-06-04 |
| §35 nombre provisional | Actualizado: "Debales" es el nombre definitivo | 2026-06-04 |
| ADR numeración colisión | Nuevos ADRs numerados desde 0004 para no solapar existentes | 2026-06-04 |
| §6 — "31 tests" y "10 migraciones" desfasados | Actualizado a 52 tests y 11 migraciones | 2026-06-05 |
| §6 — Flujo Compras espejo marcado como "no existe" | Implementado: albarán→factura desde UI de Compras | 2026-06-05 |
| §6 — ModuleRequired marcado como "no existe" | Implementado en hubs de UI | 2026-06-05 |
| §49.3 — SalesQuote no estaba en tabla de módulos | Añadido | 2026-06-05 |
| Obsidian vault desactualizado respecto al código | Vault sincronizado en cada sesión desde 2026-06-05 | 2026-06-05 |

## 47.2 Pendiente de decidir

| Tema | Opciones | Impacto |
|---|---|---|
| Multi-tenant desde inicio | Sí ahora / No hasta Fase ERP | Afecta todas las tablas con `TenantId` |
| Motor contable propio vs librería | Implementación propia / Adaptar librería .NET contable | Esfuerzo y flexibilidad |
| Plan contable por defecto | PGC España / Internacional / Configurable | Primer cliente objetivo |
| Formas de pago y remesas bancarias | SEPA / Genérico configurable | Facturación en Fase ERP-3 |
| Generación automática de asientos | Plantillas de asiento / Hardcoded por tipo de documento | Flexibilidad futura |
| Cuentas contables de clientes y proveedores | Cuenta única / Cuenta individual por tercero | PGC español exige individual |

## 47.3 Reglas para resolver pendientes

Antes de implementar cualquier elemento de la lista:

1. Documentar la decisión como ADR en `/docs/decisions`.
2. Comunicarla a Carlos para aprobación.
3. Actualizar este CLAUDE.md con el resultado.
4. Mover de "Pendiente de decidir" a "Conflictos resueltos".

---

# 48. Estilo de código — preferencias de Carlos

> Registrado 2026-05-28. Preferencias concretas que Claude debe aplicar en este proyecto.

## 48.1 LINQ

Preferir **sintaxis de query** (`from … in … where … select`) sobre método encadenado cuando la consulta tenga joins, múltiples cláusulas `where`, agrupaciones o proyecciones complejas.

Para consultas simples (un solo filtro + proyección), el estilo de método encadenado es aceptable.

```csharp
// Preferido para consultas con join o múltiples cláusulas
var result = from s in suppliers
             join c in contacts on s.Id equals c.SupplierId
             where s.IsActive && c.IsPrimary
             orderby s.Name
             select new SupplierContactDto(s.Name, c.Email);

// Aceptable para consultas simples
var names = suppliers.Where(s => s.IsActive).Select(s => s.Name);
```

## 48.2 Records e inmutabilidad

Usar `record` para DTOs, value objects y resultados de queries.  
Usar `sealed class` para entidades de dominio, handlers y repositorios.  
Nunca usar `struct` salvo justificación explícita de rendimiento medida.

## 48.3 Validación fail-fast

Validar argumentos al inicio del método. Lanzar `ArgumentException` o `InvalidOperationException` con mensaje claro antes de ejecutar lógica.

---

# 49. Patrón de implementación de módulos

> Registrado 2026-05-28. Patrón establecido durante ERP-1 (Suppliers). Aplicar siempre en módulos nuevos.

## 49.1 Orden obligatorio de implementación

```txt
1. Domain        ← entidad + value objects
2. Application   ← interfaces, DTOs, handlers
3. Infrastructure← EF config, repositorio, registro DI
4. API           ← controller con GET list/GET by id/POST/PUT
5. Web           ← lista Blazor + ficha detalle
6. NavMenu       ← enlace en sección correspondiente
7. Migración EF  ← siempre con --startup-project Debales.Api
```

## 49.2 Reglas críticas del patrón

- `IEntityRepository` hereda `IRepository<T>` — usar `new` en métodos que ocultan el base.
- `internal static ToDto(Entity)` va en `Update<Entity>Handler`, reutilizado por `GetByIdHandler`.
- Request records en controllers deben ser `public sealed record` (accesibilidad C# CS0051).
- `OwnsOne(...)` para value objects embebidos (Address, etc.) — no tabla separada.
- `HasQueryFilter(e => !e.IsDeleted)` en toda entidad con soft-delete.
- Índice único en campo de unicidad: `.HasFilter("[Field] IS NOT NULL")` si el campo es nullable.
- Namespaces de Application añadir a `_Imports.razor` para uso en Blazor.
- Handlers registrar en `Debales.Application/DependencyInjection.cs`.
- Repositorios registrar en `Debales.Infrastructure/DependencyInjection.cs`.

## 49.3 Módulos implementados

| Módulo | Estado | Migración |
|---|---|---|
| Core (Users, Roles, Permissions) | Completo | `20260527211045_InitialCreate` |
| CRM (Customers, Contacts, Activities, Notes, Opportunities) | Completo | `20260527211933_AddCrmModule` |
| Suppliers | Completo — ERP-1 | `20260528174128_AddSuppliersModule` |
| Catalog (Items, Families, UOM, TaxType) | Completo — ERP-1 | `20260528180338_AddCatalogModule` |
| Sales (Orders, DeliveryNotes, Invoices, CreditNotes, Receivables, Payments) | Completo — ERP-2/3 | `20260529075409_AddERP2Module` + `20260601201930_AddERP3Module` |
| Purchasing (Orders, DeliveryNotes, Invoices, CreditNotes, Payables, Payments) | Completo — ERP-2/3 | `20260529075409_AddERP2Module` + `20260601201930_AddERP3Module` |
| Inventory (Warehouses, Locations, StockMovements, StockBalance) | Completo — ERP-4 | `20260601212414_AddERP4Module` |
| Accounting (ChartOfAccounts, FiscalYear, Journals, Entries) | Completo — ERP-5 | `20260602195700_AddAccountingModule` |
| AI supervisada ERP (chat, anomalías, resúmenes) | Completo — ERP-6 | sin migración propia |
| Licensing | Completo — Fase 6 | `20260604121322_AddLicensingModule` |
| Docker Compose | Completo — Fase 7 | sin migración |
| SalesQuote (Presupuestos) | Completo — ERP-Quote | `20260604230558_AddSalesQuoteModule` |
| Informes contables (Balance, Libro diario, Situación) | Completo — ERP-Reports | sin migración |
| Paridad Compras/Ventas (estado pedido en albarán) | Completo — fix | sin migración |
| Asientos desde cobros/pagos (motor + plantillas seed) | Completo — 2026-06-06/07 | `20260607121801_AddPaymentAccountingTemplates` |
| AccountCode en Customer/Supplier (cascade DTO→handler→UI) | Completo — 2026-06-07 | sin migración |
| Sistema auditoría automática (`AuditEntry`, `ICurrentUserService`, UI `/configuracion/auditoria`) | Completo — 2026-06-07 | sin migración |
| Tutorial guiado (`TutorialService`, `TutorialOverlay`, 8 pasos, localStorage) | Completo — 2026-06-07 | sin migración |
| Dashboard KPIs (`Home.razor`, 6 KPIs importes € + alertas cobros/pagos vencidos) | Completo — 2026-06-07 | sin migración |
| Tests `AssignRoleHandler` (4) + `DeactivateUserHandler` (3) — cobertura completa handlers | Completo — 2026-06-07 | sin migración |
| NumberSeries (series documentales configurables, UI `/configuracion/series`) | Completo — 2026-06-07 | `20260607141730_AddNumberSeriesModule` |
| NumberSeries cableado en handlers (13 handlers de creación de documentos) | Completo — 2026-06-09 | `20260609153343_AddNumberSeriesSeed` |
| API REST NumberSeries (`NumberSeriesController` GET/POST/PUT) | Completo — 2026-06-09 | sin migración |
| Middleware licencias API (`RequiresModuleAttribute`, 15 controllers) + Web (`ModuleRouteGuard` en MainLayout) | Completo — 2026-06-09 | sin migración |
| Tarifas y códigos de artículo por tercero (`PriceList`, `ItemPrice`, `SupplierItemCode`, `CustomerItemCode`) — Domain + 12 handlers + API (PriceListsController, sub-rutas en Suppliers/Customers) + UI (Tarifas.razor, TarifaDetalle.razor, tabs Códigos en Proveedor/Cliente) | Completo — 2026-06-09 | `20260609163530_AddPriceListModule` |
| `PaymentTerm` + `PaymentTermLine` (condiciones de pago) — Domain + Application (5 handlers) + Infrastructure + API + UI `/configuracion/condiciones-pago` + seed 7 condiciones | Completo — 2026-06-10 | `20260609223341_AddPaymentMethodModule` |
| `PaymentMethod` (formas de pago) — Domain + Application (5 handlers + 7 tests) + Infrastructure + API + UI `/configuracion/formas-pago` + seed 6 formas; FK en Customer/Supplier | Completo — 2026-06-10 | `20260609225012_AddPaymentTermAndMethodSeed` |
| `BankAccount` (cuentas bancarias) — Domain + Application (5 handlers) + Infrastructure + API (`BankAccountsController`) + UI `/contabilidad/cuentas-bancarias`; FK opcional a Account | Completo — 2026-06-10 | `20260609225537_AddBankAccountModule` |
| `AdjustStock` command+handler — ajusta stock a cantidad objetivo, calcula delta, genera `StockMovement` de tipo Adjustment; botón "Ajustar" en UI de Saldos de stock | Completo — 2026-06-10 | sin migración |
| `Item.MinimumStock` (decimal nullable) — campo en Domain + Application + Infrastructure (HasPrecision) + API + UI (ficha + lista); badge "Bajo mínimo" en SaldosStock; alerta KPI en Dashboard | Completo — 2026-06-10 | `20260610194228_AddMinimumStockToItems` |
| `InventoryCount` + `InventoryCountLine` — sesión de recuento físico: crear → añadir artículos (con stock sistema) → registrar cantidades contadas → cerrar (genera ajustes automáticos para diferencias). Domain + Application (6 handlers) + Infrastructure + API (`InventoryCountsController`) + UI (`ConteoInventario.razor` + `ConteoInventarioDetalle.razor`) | Completo — 2026-06-10 | `20260610201622_AddInventoryCountModule` |
| `MinimumStock` en `Item` — campo nullable decimal en Domain + Application (DTOs, Commands) + Infrastructure + migración + API; alerta "Bajo mínimo" en `SaldosStock.razor`; KPI en Dashboard | Completo — 2026-06-10 | `20260610194228_AddMinimumStockToItems` |
| `CashAccount` (cajas) — Domain + Application (5 handlers) + Infrastructure + API (`CashAccountsController`) + UI `/contabilidad/cajas`; FK opcional a Account | Completo — 2026-06-10 | `20260610203401_AddCashAccountModule` |
| `Remittance` + `RemittanceLine` (remesas bancarias) — Domain (estado: Draft→Sent→Confirmed/Failed) + Application (8 commands + 2 queries + handlers) + Infrastructure + API (`RemittancesController`) + UI (`Remesas.razor` + `RemesaDetalle.razor`); liquida Receivables/Payables al confirmar | Completo — 2026-06-11 | `20260611141125_AddRemittanceModule` |
| Informe vencimientos aging (`GetReceivablesAgingHandler` + `GetPayablesAgingHandler`) — buckets Corriente/1-30/31-60/61-90/+90; `AgingReportDto` en `Application.Common`; API `ReportsController` (`/api/reports/receivables-aging`, `/api/reports/payables-aging`); UI `/contabilidad/vencimientos` (2 tabs, 5 bucket cards, tabla coloreada por vencimiento) | Completo — 2026-06-11 | sin migración |
| Posición de tesorería (`GetTreasuryPositionHandler`) — suma saldos de `BankAccount` + `CashAccount`; `TreasuryPositionDto` en `AccountingDtos`; API `ReportsController` (`/api/reports/treasury-position`); UI `/contabilidad/tesoreria` (3 KPIs, tablas banco/caja); KPI tesorería en Dashboard `Home.razor` | Completo — 2026-06-11 | sin migración |
| Estado de cuenta cliente/proveedor (`GetCustomerStatementHandler` + `GetSupplierStatementHandler`) — movimientos ordenados por fecha (facturas, rectificativas, cobros/pagos) con saldo acumulado; `StatementDto` + `StatementLineDto` en `Application.Common`; `GetByCustomerForStatementAsync`/`GetBySupplierForStatementAsync` en 6 repos; API `ReportsController`; UI `/contabilidad/estado-cuenta-clientes` + `/contabilidad/estado-cuenta-proveedores`; tab "Estado de cuenta" en `CustomerDetail.razor` + `SupplierDetail.razor`; tests (7+5+4) | Completo — 2026-06-11 | sin migración |
| `ItemDetail.razor` tabs — "Información" (layout existente), "Movimientos" (historial stock con saldo acumulado, lazy, usa `GetStockMovementsHandler` con `ItemId`), "Saldos por almacén" (saldos por almacén, lazy, usa `GetStockBalanceHandler`) | Completo — 2026-06-11 | sin migración |
| Tests `GetItemPriceHandler` (6) + `ConvertQuoteToOrderHandler` (9) — cobertura resolución precios y ciclo presupuesto→pedido | Completo — 2026-06-11 | sin migración |
| Tab "Pedidos" en `CustomerDetail.razor` — historial lazy de `SalesOrder` por cliente (badge estado, link a ficha) | Completo — 2026-06-11 | sin migración |
| Tab "Pedidos" en `SupplierDetail.razor` — historial lazy de `PurchaseOrder` por proveedor (badge estado, link a ficha) | Completo — 2026-06-11 | sin migración |
| `SupplierContact` (contactos de proveedor) — Domain + Application (4 handlers: Add/Update/Deactivate/GetBySupplier) + Infrastructure + API (sub-rutas GET/POST/PUT/DELETE en `SuppliersController`) + Web (tab "Contactos" lazy en `SupplierDetail.razor`, modal añadir/editar, confirmación baja) | Completo — 2026-06-12 | `20260612141224_AddSupplierContactModule` |
| `Documents` module — Domain (`Document`, `DocumentType`) + Application (8 handlers: Create/Update/Deactivate/GetAll/GetById para Document + Create/Update/GetAll para DocumentType) + Infrastructure (2 configuraciones + 2 repositorios) + API (`DocumentsController`, `DocumentTypesController`) + Web (`Documentos.razor` `/documentos`, `DocumentoDetalle.razor` `/documentos/{id}`, `TiposDocumento.razor` `/configuracion/tipos-documento`) + NavMenu (sección Documentos + sublink Tipos de documento en Configuración) | Completo — 2026-06-13 | `20260613120000_AddDocumentsModule` (manual) |
| Tab "Documentos" en `CustomerDetail.razor` y `SupplierDetail.razor` — lazy, filtra por CustomerId/SupplierId via `GetDocumentsHandler` | Completo — 2026-06-14 | sin migración |
| `AIGovernance` module (§42.12) — Domain (`AIRule`, `AIKnowledgeBase`, `AIActionProposal`, `AIActionApproval`, `AIExecutionLog`) + Application (13 handlers, 5 repositorios interfaz) + Infrastructure (5 repos EF + 5 configs) + API (`AIGovernanceController`) + Web (`Propuestas.razor` `/ai/propuestas`, `PropuestaDetalle.razor` `/ai/propuestas/{id}`, `AIReglas.razor` `/configuracion/ai-reglas`, `AIConocimiento.razor` `/configuracion/ai-conocimiento`) + NavMenu | Completo — 2026-06-14 | `20260614090000_AddAIGovernanceModule` (manual) |
| Multi-tenant estructural — `TenantId` (Guid?) en `AuditableEntity`, `ITenantService` + `HttpContextTenantService`, `SetTenantIds()` en DbContext.SaveChangesAsync; filtros de query diferidos | Completo (estructura) — 2026-06-14 | `20260614160000_AddTenantIdToBusinessEntities` (manual) |
| Subida de archivos documentos — `IFileStorageService` / `LocalFileStorageService` (configurable), `UploadDocumentFileHandler`, `POST /api/documents/{id}/upload`, UI InputFile en `DocumentoDetalle.razor` | Completo — 2026-06-14 | sin migración |
| Integración Chat IA → propuesta — botón "Guardar como propuesta IA" en `IA.razor`, genera `AIActionProposal` con el último par pregunta/respuesta del chat ERP | Completo — 2026-06-14 | sin migración |
| Importación CSV masiva — `CsvParser` + `ImportResult` en Application.Common; `ImportCustomersHandler`, `ImportSuppliersHandler`, `ImportItemsHandler`; `ImportController` (`POST /api/import/*`); `Importacion.razor` `/configuracion/importacion`; `ImportResultCard` componente compartido; 8 tests | Completo — 2026-06-14 | sin migración |
