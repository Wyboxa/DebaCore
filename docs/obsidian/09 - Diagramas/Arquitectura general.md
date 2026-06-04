---
type: diagram
module: cross
layer: cross
status: implemented
related:
  - 01 - Arquitectura
  - Relacion de modulos
---

# Diagrama: Arquitectura general

```mermaid
graph TB
    subgraph Clientes
        Browser[Navegador]
        APIClient[Cliente API externo]
    end

    subgraph Debales.Web
        Blazor[Blazor Server\nInteractiveServer]
    end

    subgraph Debales.Api
        API[ASP.NET Core\nREST API + JWT]
    end

    subgraph Debales.Application
        Handlers[Handlers CQRS\n~80 handlers]
        DTOs[DTOs Records]
        IRepos[Interfaces\nRepositorios]
    end

    subgraph Debales.Domain
        Entities[Entidades\n51 clases]
        VO[Value Objects\nAddress, Email...]
        Rules[Reglas dominio\nMétodos entidad]
    end

    subgraph Debales.Infrastructure
        Repos[Repositorios\nEF Core]
        DbCtx[ApplicationDbContext\n48 DbSets]
        Migrations[10 Migraciones]
        Seeds[Seeders\nDbSeeder + Demo]
        Security[JWT + Password\nHasher]
    end

    subgraph Debales.AI
        AIService[AIService]
        Claude[ClaudeProvider]
        Mock[MockProvider]
        Prompts[PromptBuilder]
    end

    subgraph Database
        SQL[(SQL Server\nDebalesDb)]
    end

    Browser --> Blazor
    APIClient --> API
    Blazor --> Handlers
    API --> Handlers
    Blazor --> Repos
    Handlers --> IRepos
    Handlers --> Entities
    IRepos --> Repos
    Handlers --> AIService
    AIService --> Claude
    AIService --> Mock
    Repos --> DbCtx
    DbCtx --> SQL
```

## Notas sobre el diagrama

- Blazor accede directamente a repositorios además de handlers (dependencia real en código)
- Los handlers son Scoped, no singleton
- La selección Claude/Mock se hace en arranque por configuración
