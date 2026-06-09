---
type: module
module: licenciamiento
layer: cross
status: implemented
source:
  - src/Debales.Domain/Licensing/
  - src/Debales.Application/Licensing/
  - src/Debales.Infrastructure/Persistence/Repositories/Licensing/
  - src/Debales.Infrastructure/Services/LicenseService.cs
  - src/Debales.Api/Controllers/LicensesController.cs
  - src/Debales.Api/Controllers/SubscriptionPlansController.cs
  - src/Debales.Web/Components/Pages/Licencia/Licencia.razor
related:
  - License
  - LicenseModule
  - SubscriptionPlan
---

# Módulo Licenciamiento

## Estado

CLAUDE.md actualizado el 2026-06-04 — Licensing declarado como ✓ COMPLETA en §46 y §49.3.

## Qué problema resuelve

Control de licencias de la plataforma: planes de suscripción, activación, módulos contratados, validación de vigencia y estados de licencia.

## Estado

**Implementado** — migración `AddLicensingModule` (2026-06-04).

## Entidades principales

| Entidad | Descripción |
|---------|-------------|
| [[License]] | Licencia con clave, empresa, plan, fechas y módulos contratados |
| [[LicenseModule]] | Módulo específico concedido en una licencia |
| [[SubscriptionPlan]] | Plan de suscripción con límites de usuarios, módulos y flag IA |

## Reglas de dominio

- `ExpiresAt > StartsAt` — validado en `License.Create()`
- `License.IsValid()` = Status Active o Trial + fechas vigentes
- `License.HasModule(code)` = IsValid + módulo en lista
- Activar licencia Expired o Suspended lanza excepción
- `CheckAndExpire()` — marca como Expired si fecha superada

## Estados de licencia

`Trial → Active | Expired | Suspended`

## Handlers

| Handler | Tipo | Descripción |
|---------|------|-------------|
| `GetCurrentLicenseHandler` | Query | Obtiene licencia vigente actual |
| `GetSubscriptionPlansHandler` | Query | Lista planes disponibles |
| `ActivateLicenseHandler` | Command | Activa una licencia con clave, plan y módulos |

## Controllers

| Controller | Ruta | Endpoints |
|------------|------|-----------|
| `LicensesController` | `api/licenses` | GET `/current`, POST `/activate` |
| `SubscriptionPlansController` | `api/subscription-plans` | GET / |

## Página Blazor

| Página | Ruta | Estado |
|--------|------|--------|
| `Licencia.razor` | `/licencia` | Implementada — muestra estado, plan, módulos y formulario de activación |

## Repositorios

- `ILicenseRepository` → `LicenseRepository`
- `ISubscriptionPlanRepository` → `SubscriptionPlanRepository`

## Servicios

- `ILicenseService` → `LicenseService` — lógica de validación y verificación de expiración

## Plans sembrados (inferido — SubscriptionPlan.ForSeed existe)

Los planes se definen con `ForSeed` factory method. Los datos exactos no se leen en el seeder principal, pero la migración puede incluir data seeds.

## Lo que está completo

- Entidades de dominio completas
- Handlers de activación y consulta
- UI de licencia con visualización de estado, plan y módulos
- Formulario de activación en UI
- Validación de vigencia

## Enforcement de licencia en UI (implementado 2026-06-05)

`ModuleRequired.razor` — componente Razor shared que actúa como guard:

```razor
<ModuleRequired Module="Sales">
    @* contenido del hub/página *@
</ModuleRequired>
```

Comportamiento:
- Si no hay licencia activa → permite el acceso (nueva instalación)
- Si hay licencia pero el módulo no está en `LicenseModules` → muestra pantalla de bloqueo con enlace a `/licencia`
- Si el módulo está activo → renderiza `ChildContent` normalmente

Aplicado en: `Ventas.razor`, `Compras.razor`, `Inventario.razor`, `Facturacion.razor`, `Analitica.razor`

## Middleware de licencias en API (implementado 2026-06-09)

`RequiresModuleAttribute` en `src/Debales.Api/Filters/RequiresModuleAttribute.cs` — implementa `IFilterFactory` para inyectar `ILicenseService` vía DI:

```csharp
[RequiresModule("Sales")]
public sealed class SalesOrdersController : ControllerBase { ... }
```

Comportamiento:
- Sin licencia válida → permite todo (nueva instalación)
- Licencia válida + módulo no contratado → HTTP 403 `{ "error": "Módulo 'X' no licenciado." }`

Aplicado a 15 controllers: Sales (6), Purchasing (5), Inventory (2), Accounting (1), AI (1).

## Middleware de licencias en Web (implementado 2026-06-09)

`ModuleRouteGuard.razor` en `MainLayout` — cubre automáticamente todas las páginas individuales sin modificarlas:

```razor
<ModuleRouteGuard>
    @Body
</ModuleRouteGuard>
```

Tabla de rutas protegidas: `/ventas`, `/facturacion/ventas*`, `/facturacion/rectificativas-venta*`, `/compras`, `/facturacion/compras*`, `/facturacion/rectificativas-compra*`, `/inventario`, `/contabilidad`, `/ia`, `/analitica`.

Patrón optimista: muestra contenido mientras comprueba (igual que `ModuleRequired`). Reactivo a `LocationChanged`.

## Lo que falta

- Modo offline con expiración controlada
- Renovación desde UI
