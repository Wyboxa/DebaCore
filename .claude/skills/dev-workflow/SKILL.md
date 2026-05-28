# Skill: Nuevo módulo Debales

Implementa un módulo nuevo siguiendo el patrón de 7 capas del proyecto Debales.

## Orden de implementación

1. **Domain** — entidad principal + value objects si aplica  
   Namespace: `Debales.Domain.<Module>`  
   Hereda: `AuditableEntity`  
   Patrón: constructor privado, factory `Create(...)`, métodos `Update(...)`, `Deactivate(...)`

2. **Application — Interfaces**  
   `I<Module>Repository` en `Debales.Application.<Module>`  
   Hereda: `IRepository<TEntity>`  
   Métodos mínimos: `GetByIdAsync`, `SearchAsync`, `ExistsByXAsync` si hay unicidad

3. **Application — DTOs**  
   `<Module>SummaryDto` — campos para lista  
   `<Module>DetailDto` — campos completos para ficha

4. **Application — Handlers**  
   Commands: `Create<Module>Handler`, `Update<Module>Handler`  
   Queries: `Get<Module>sHandler`, `Get<Module>ByIdHandler`  
   Patrón: `internal static ToDto(entity)` en UpdateHandler, reutilizado por GetByIdHandler  
   Registrar en `Debales.Application/DependencyInjection.cs`

5. **Infrastructure**  
   `Configurations/<Module>/<Entity>Configuration.cs` — EF Core `IEntityTypeConfiguration<T>`  
   `Repositories/<Module>/<Entity>Repository.cs` — implementa `I<Module>Repository`  
   Registrar en `Debales.Infrastructure/DependencyInjection.cs`  
   Añadir `DbSet<T>` en `ApplicationDbContext`

6. **API**  
   `Controllers/<Module>Controller.cs`  
   GET list (search + page), GET by id, POST, PUT  
   Request records: `public sealed record` (accesibilidad requerida)  
   Decorar con `[Authorize]`

7. **Web (Blazor)**  
   `Pages/<Module>/<Entities>.razor` — lista con búsqueda, paginación, modal de creación  
   `Pages/<Module>/<Entity>Detail.razor` — ficha con tabs, modal de edición  
   Añadir ruta en `NavMenu.razor`  
   Añadir namespaces en `_Imports.razor`

## Migración EF

```powershell
dotnet ef migrations add Add<Module>Module `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj

dotnet ef database update `
  --project src\Debales.Infrastructure\Debales.Infrastructure.csproj `
  --startup-project src\Debales.Api\Debales.Api.csproj
```

## Reglas críticas

- `Debales.Web` no tiene EF Design — usar siempre `Debales.Api` como startup project.
- Detener procesos Debales antes de compilar si hay DLLs bloqueados.
- `OwnsOne(...)` para Value Objects embebidos (Address, etc.).
- `HasQueryFilter(e => !e.IsDeleted)` en entidades con soft-delete.
- Índice único en campos de unicidad con null filter: `.HasFilter("[TaxId] IS NOT NULL")`.
