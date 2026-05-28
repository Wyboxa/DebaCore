# Arquitectura general — Debales

## Enfoque

Monolito modular. No microservicios en el MVP.

**Motivo:** Menor complejidad operativa, más rápido de desarrollar y mantener para un equipo pequeño. Permite escalar por módulos cuando haya razón real para hacerlo.

## Estructura de proyectos (real — 2026-05-28)

```
src/
├── Debales.Api/              # Endpoints HTTP, controllers mínimos
├── Debales.Web/              # Blazor Server — UI
├── Debales.Application/      # Casos de uso, handlers, DTOs, validaciones
│   ├── Core/                 # Usuarios, roles
│   └── CRM/                 # Clientes, contactos, actividades, notas, oportunidades
├── Debales.Domain/           # Entidades, Value Objects, reglas de dominio
│   ├── Core/                 # User, Role, Permission, SystemModule, AuditEntry
│   └── CRM/                 # Customer, Contact, Activity, Note, Opportunity
├── Debales.Infrastructure/   # Persistencia, EF Core, repositorios, seguridad
├── Debales.AI/               # Abstracción IA (IAIProvider + MockAIProvider)
└── Debales.Modules/
    └── Core/                 # Manifiesto del módulo Core (IModule)

tests/
├── Debales.Domain.Tests/
├── Debales.Application.Tests/
└── Debales.Integration.Tests/
```

> **Nota:** La lógica de CRM vive en las capas principales (Domain, Application, Infrastructure),
> no como proyecto de módulo separado. Solo `Core` tiene proyecto de módulo por necesitar manifiesto
> en tiempo de carga. Los módulos futuros seguirán el mismo patrón.

## Módulos planificados (futuros)

A medida que se implementen, cada módulo añadirá sus entidades en las capas existentes
y, si necesita manifiesto de registro, un proyecto `Debales.Modules.[Nombre]/`.

```
Módulos núcleo planificados:
├── Suppliers    (ERP-1)
├── Catalog      (ERP-1)
├── Sales        (ERP-2 / ERP-3)
├── Purchasing   (ERP-2 / ERP-3)
├── Inventory    (ERP-4)
├── Accounting   (ERP-5)
└── Documents    (backlog)

Módulo transversal:
└── AI           (Fase 4)
```

Ver: [module-boundaries.md](module-boundaries.md)

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
