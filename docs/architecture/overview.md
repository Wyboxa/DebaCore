# Arquitectura general — Debales

## Enfoque

Monolito modular. No microservicios en el MVP.

**Motivo:** Menor complejidad operativa, más rápido de desarrollar y mantener para un equipo pequeño. Permite escalar por módulos cuando haya razón real para hacerlo.

## Estructura de proyectos

```
src/
├── Debales.Api/              # Endpoints HTTP, controllers mínimos
├── Debales.Web/              # Blazor Server — UI
├── Debales.Application/      # Casos de uso, handlers, DTOs, validaciones
├── Debales.Domain/           # Entidades, Value Objects, reglas de dominio
├── Debales.Infrastructure/   # Persistencia, EF Core, integraciones externas
├── Debales.AI/               # Orquestación IA, providers, prompts, agentes
└── Debales.Modules/
    ├── Core/                 # Usuarios, roles, permisos, módulos, auditoría
    ├── CRM/                  # Clientes, contactos, actividades, oportunidades
    ├── Documents/            # Documentos, versiones, comentarios
    └── Billing/              # Facturación (fase futura)

tests/
├── Debales.Domain.Tests/
├── Debales.Application.Tests/
└── Debales.Integration.Tests/
```

## Flujo de una petición

```
Request HTTP
  → Controller (Debales.Api)
    → Command/Query (Debales.Application)
      → Handler/Service
        → Domain (reglas)
        → Repository (Debales.Infrastructure)
  → Response
```

## Responsabilidades por capa

| Capa           | Contiene                                             | Prohibido                                      |
|----------------|------------------------------------------------------|------------------------------------------------|
| Domain         | Entidades, Value Objects, eventos, reglas puras      | BD, HTTP, IA, infraestructura                  |
| Application    | Casos de uso, handlers, DTOs, validaciones           | SQL directo, lógica visual, provider IA        |
| Infrastructure | Repositorios, EF Core, email, archivos               | Reglas de negocio                              |
| AI             | Providers, prompts, agentes, context builders        | Reglas críticas irrevisables, escritura directa|
| Api            | Controllers mínimos, mapeo request/response          | Lógica de negocio                              |
| Web (Blazor)   | Componentes UI, servicios de presentación            | Lógica de negocio, acceso directo a BD         |
