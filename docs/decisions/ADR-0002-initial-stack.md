# ADR-0002 — Stack tecnológico inicial

## Estado

Aceptada

## Contexto

Se necesita definir el stack antes de crear la solución .NET para fijar namespaces, estructura de proyectos y convenciones.

## Decisión

### Backend

- **Lenguaje:** C# (.NET 8 o superior)
- **Framework:** ASP.NET Core
- **Arquitectura:** Clean Architecture por capas (Domain, Application, Infrastructure, Api)
- **ORM:** Entity Framework Core (opción principal); Dapper para consultas complejas si aplica
- **Tests:** xUnit

### Frontend

- **Framework:** Blazor Server
- **Motivo:** Mismo ecosistema .NET/C#, sin framework JS separado, DI y modelos compartidos con el backend, real-time via SignalR adecuado para dashboards empresariales. Puede evolucionar a Blazor Hybrid o WASM si se necesita modo offline.

### Base de datos

- **Principal:** SQL Server (experiencia previa del equipo)
- **Alternativa:** PostgreSQL si el despliegue lo requiere
- **Regla:** Toda evolución de BD mediante migraciones EF Core. Sin cambios manuales en producción.

### IA

- Abstracción `IAIProvider` desacoplada del dominio
- Proveedores: Claude (Anthropic), OpenAI, AzureOpenAI, LocalModel, Mock
- El dominio no importa ningún SDK de IA directamente

### Nombres de proyectos

```
Debales.Api
Debales.Web
Debales.Application
Debales.Domain
Debales.Infrastructure
Debales.AI
Debales.Modules.Core
Debales.Modules.CRM
Debales.Modules.Documents
```

## Consecuencias

- No se introducen microservicios salvo justificación fuerte documentada en un ADR.
- No se mezcla lógica de negocio en controllers ni en componentes Blazor.
- Los secretos y connection strings nunca van al repositorio.

## Alternativas consideradas

- React: descartado para MVP por overhead de mantener dos ecosistemas (JS + .NET).
- Angular: mismo motivo que React.
- Blazor WASM: descartado para MVP por mayor complejidad de despliegue; puede adoptarse en Fase 7.
- PostgreSQL como principal: SQL Server elegido por experiencia previa del equipo.
